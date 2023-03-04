using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.IO;
using TechnoEgypt.DTOS;
using TechnoEgypt.Models;

namespace TechnoEgypt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dBContext;
        private readonly IWebHostEnvironment env;

        public UserController(AppDBContext dBContext, IWebHostEnvironment env)
        {
            _dBContext = dBContext;
            this.env = env;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto model)
        {
            Response<LoginToReturnDto> response = new();
            response.StatusCode = ResponseCode.success;
            var data = _dBContext.Parents.Where(w => w.UserName == model.UserName && model.PhoneNumber == w.FatherPhoneNumber
                                                && w.Children.Any(c => c.IsActive))
                                                .Include(w => w.Children).ThenInclude(w => w.ChildCertificates);
            if (data.FirstOrDefault() == null)
            {
                response.StatusCode = ResponseCode.notFound;
                response.Message = "User Not Found";
                return Ok(response);
            }
            var child = data.FirstOrDefault().Children.FirstOrDefault(w => w.IsActive);
            var age = (int)((DateTime.Now - child.DateOfBirth).TotalDays / 365);
            var stage = _dBContext.Stages.FirstOrDefault(w => w.AgeFrom <= age && w.AgeTo >= age);
            response.Data = new LoginToReturnDto()
            {
                Id = child.Id,
                Group_Id = stage != null ? stage.Id : 0,
                Group_Name = model.languageId == 0 ? stage?.Name : stage.ArName,
                Certificates = child.ChildCertificates.Select(w => new Certificat() { Id = w.Id, Image_Url = w.FileURL }).ToList(),
                school = child.SchoolName,
                childern = data.FirstOrDefault().Children.Where(w => w.IsActive).Select(w => new ChildData() { Id = w.Id, Name = w.Name }).ToList()
            };
            response.Message = "success";
            return Ok(response);
        }
        [HttpPost("GetUserChildById")]
        public IActionResult GetUserChildById(BaseDto model)
        {
            var response = new Response<LoginToReturnDto>();
            var child = _dBContext.children.Where(w => w.Id == model.UserId && w.IsActive).Include(w => w.ChildCertificates).FirstOrDefault();
            var age = (int)((DateTime.Now - child.DateOfBirth).TotalDays / 365);
            var stage = _dBContext.Stages.FirstOrDefault(w => w.AgeFrom >= age && w.AgeTo <= age);
            response.Data = new LoginToReturnDto()
            {
                Id = child.Id,
                Group_Id = stage != null ? stage.Id : 0,
                Group_Name = model.languageId == 0 ? stage?.Name : stage.ArName,
                Certificates = child.ChildCertificates.Select(w => new Certificat() { Id = w.Id, Image_Url = w.FileURL }).ToList(),
                school = child.SchoolName,
                childern = _dBContext.children.Where(w => w.ParentId == child.ParentId && w.IsActive).Select(w => new ChildData() { Id = w.Id, Name = w.Name }).ToList()

            };
            response.Message = "success";
            return Ok(response);
        }
        [HttpPost("StationData")]
        public IActionResult StationData(languageDto model)
        {
            var response = new Response<List<StationDataDto>>();
            response.Message = "success";
            response.StatusCode = ResponseCode.success;
            var data = _dBContext.station.Include(w => w.childCVDatas).ToList();
            response.Data = data.Select(w => new StationDataDto()
            {
                Id = w.Id,
                Title = model.languageId == 0 ? w.Title : w.ArTitle,
                Desc = model.languageId == 0 ? w.Description : w.ArDescription,
                available = w.IsAvilable,
                Icon = w.IconURL
            }).ToList();
            return Ok(response);
        }
        [HttpPost("UpdateChildCVData")]
        public IActionResult UpdateChildCVData([FromForm] ChildCVoSaveDto model)
        {
            var FilePath = "\\Files\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Millisecond.ToString() + model.File.FileName;
            var path = env.WebRootPath + FilePath;
            using (FileStream fs = System.IO.File.Create(path))
            {
                model.File.CopyTo(fs);
            }
            var station = _dBContext.childCVData.FirstOrDefault(w => w.ChildId == model.UserId && w.stationId == model.StationId);
            if (station == null)
            {
                station = new ChildCVData()
                {
                    stationId = model.StationId,
                    Date = model.Date,
                    ChildId = model.UserId.Value,
                    Name = model.Name,
                    FileURL = FilePath,
                    Note = model.Note
                };
                _dBContext.childCVData.Add(station);
            }
            else
            {
                station.Date = model.Date;
                station.Name = model.Name;
                station.Note = model.Note;
                station.FileURL = FilePath;
                _dBContext.Entry<ChildCVData>(station).State = EntityState.Modified;
            }
            _dBContext.SaveChanges();
            return Ok();
        }
        [HttpPost("GetUserStatistic")]
        public IActionResult GetUserStatistic(BaseDto model)
        {
            var stages = _dBContext.Stages.Include(w => w.CourseCategories).ThenInclude(w => w.Courses).ThenInclude(w => w.ChildCourses);
            List<StageSkillsDto> data = new List<StageSkillsDto>();
            foreach (var item in stages)
            {
                var st = new StageSkillsDto();
                st.Name = model.languageId == 0 ? item.Name : item.ArName;
                st.Id = item.Id;
                var courses = item.CourseCategories.SelectMany(w => w.Courses).ToList();
                if (courses.Count > 0)
                {
                    var userCourses = courses.Where(w => w.ChildCourses.Any(w => w.ChildId == model.UserId)).ToList();
                    st.skills = new()
                    {
                        new SkillDto(){
                            SkillId=SkillType.DataCollectionandAnalysis,
                            Name=model.languageId==0?"Data Collection and Analysis":"تجميع البيانات وتحليلها",
                            Precentage=GetPresentage(courses.Count(w=>w.DataCollectionandAnalysis),userCourses.Count(w=>w.DataCollectionandAnalysis)),
                        },
                        new SkillDto(){
                            SkillId=SkillType.CriticalThinking,
                            Name=model.languageId==0?"Critical Thinking":"التفكير النقدي",
                            Precentage=GetPresentage(courses.Count(w=>w.CriticalThinking),userCourses.Count(w=>w.CriticalThinking)),
                        },
                        new SkillDto(){
                            SkillId=SkillType.Planning,
                            Name=model.languageId==0?"Planning":"تخطيط",
                            Precentage=GetPresentage(courses.Count(w=>w.Planning),userCourses.Count(w=>w.Planning)),
                        },
                        new SkillDto(){
                            SkillId=SkillType.MathematicalReasoning,
                            Name=model.languageId==0?"Mathematical Reasoning":"المنطق الرياضي",
                            Precentage=GetPresentage(courses.Count(w=>w.MathematicalReasoning),userCourses.Count(w=>w.MathematicalReasoning)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.Innovation,
                            Name=model.languageId==0?"Innovation":"ابتكار",
                            Precentage=GetPresentage(courses.Count(w=>w.Innovation),userCourses.Count(w=>w.Innovation)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.LogicalThinking,
                            Name=model.languageId==0?"Logical Thinking":"التفكير المنطقي",
                            Precentage=GetPresentage(courses.Count(w=>w.LogicalThinking),userCourses.Count(w=>w.LogicalThinking)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.CognitiveAbilities,
                            Name=model.languageId==0?"Cognitive Abilities":"القدرات المعرفية",
                            Precentage=GetPresentage(courses.Count(w=>w.CognitiveAbilities),userCourses.Count(w=>w.CognitiveAbilities)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.ProblemSolving,
                            Name=model.languageId==0?"Problem Solving":"حل المشاكل",
                            Precentage=GetPresentage(courses.Count(w=>w.ProblemSolving),userCourses.Count(w=>w.ProblemSolving)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.SocialLifeSkills,
                            Name=model.languageId==0?"Social Life Skills":"مهارات الحياة الاجتماعية",
                            Precentage=GetPresentage(courses.Count(w=>w.SocialLifeSkills),userCourses.Count(w=>w.SocialLifeSkills)),
                        },
                         new SkillDto(){
                            SkillId=SkillType.ScientificResearch,
                            Name=model.languageId==0?"Scientific Research":"بحث علمي",
                            Precentage=GetPresentage(courses.Count(w=>w.ScientificResearch),userCourses.Count(w=>w.ScientificResearch)),
                        }
                    };
                    data.Add(st);
                }
            }

            return Ok(data);
        }
        private int GetPresentage(double all, double part)
        {
            var prec = (part / all) * 100;
            return (int)prec;
        }

    }
    enum SkillType
    {
        DataCollectionandAnalysis = 0,
        CriticalThinking,
        Planning,
        MathematicalReasoning,
        Innovation,
        LogicalThinking,
        CognitiveAbilities,
        ProblemSolving,
        SocialLifeSkills,
        ScientificResearch
    }
    class StageSkillsDto
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<SkillDto> skills { get; set; }

    }
    class SkillDto
    {
        public SkillType SkillId { get; set; }
        public string Name { get; set; }
        public int Precentage { get; set; }
    }
}
