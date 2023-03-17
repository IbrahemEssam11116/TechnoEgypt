using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.DTOS;
using TechnoEgypt.Models;

namespace TechnoEgypt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserDbContext _dBContext;

        public CourseController(UserDbContext dBContext)
        {
            _dBContext = dBContext;
        }
        [HttpPost("GetCourseById")]
        public IActionResult GetCourseById(BaseDto model)
        {
            var response = new Response<CourseDto>();
            var course = _dBContext.Courses.Where(w => w.Id == model.Id).Include(w => w.ChildCourses).FirstOrDefault();
            if (course == null)
            {
                response.StatusCode = ResponseCode.notFound;
                response.Message = "Course Not Found";
                return Ok(response);
            }
            response.StatusCode = ResponseCode.success;
            response.Message = "success";
            response.Data = new CourseDto()
            {
                Id = course.Id,
                Desc =model.languageId==0? course.Descripttion:course.ArDescripttion,
                Title =model.languageId==0? course.Name:course.ArName,
                Image_URL = course.ImageURL,
                IsAvailable = !course.ChildCourses.Any(w => w.ChildId == model.UserId),
                CognitiveAbilities = course.CognitiveAbilities,
                CriticalThinking = course.CriticalThinking,
                DataCollectionandAnalysis = course.DataCollectionandAnalysis,
                Innovation = course.Innovation,
                LogicalThinking = course.LogicalThinking,
                MathematicalReasoning = course.MathematicalReasoning,
                Planning = course.Planning,
                ProblemSolving = course.ProblemSolving,
                ScientificResearch = course.ScientificResearch,
                SocialLifeSkills = course.SocialLifeSkills
            };
            return Ok(response);
        }

        [HttpPost("GetTrakeDataById")]
        public IActionResult GetTrakeDataById(BaseDto model)
        {
            var response = new Response<List<TrackCoursresDto>>();
            var courses = _dBContext.Courses.Where(w => w.CourseCategoryId == model.Id).Include(w => w.courseTool).Include(w=>w.ChildCourses);
            response.StatusCode = ResponseCode.success;
            response.Message = "success";
            response.Data = courses.Select(w => new TrackCoursresDto()
            {
                Course_Title = w.Name,
                Id = w.Id,
                Tool =model.languageId==0? w.courseTool.Name:w.courseTool.ArName,
                Track_Id = w.CourseCategoryId,
                ValidFrom = w.ValidFrom,
                ValidTo = w.ValidTo,
                IsAvailable = !w.ChildCourses.Any(w => w.ChildId == model.UserId)

            }).ToList();
            return Ok(response);
        }
        [HttpPost("AssignaCourseToUser")]
        public IActionResult AssignaCourseToUser(BaseDto model)
        {
            ChildCourse childCourse = new ChildCourse()
            {
                CourseId = model.Id.Value,
                ChildId = model.UserId.Value,

            };
            _dBContext.childCourses.Add(childCourse);
            try
            {
                _dBContext.SaveChanges();
                var response = new Response<string>();
                response.StatusCode = ResponseCode.success;
                response.Message = "success";
                response.Data = "success";
                return Ok(response);
            }
            catch
            {
                var response = new Response<string>();
                response.StatusCode = ResponseCode.notFound;
                response.Message = "User Or Course Not Found";
                response.Data = "User Or Course Not Found";
                return Ok(response);

            }
        }

        [HttpPost("GetRoadMap")]
        public IActionResult GetRoadMap(BaseDto model)
        {
            var response = new Response<List<RoadmapDto>>();
            response.StatusCode = ResponseCode.success;
            response.Message = "success";
            var data = _dBContext.Stages.Include(w => w.CourseCategories).ThenInclude(w => w.Courses).ThenInclude(w => w.ChildCourses);
            response.Data = data.Select(w => new RoadmapDto()
            {
                Name =model.languageId==0? w.Name:w.ArName,
                AgeFrom = w.AgeFrom,
                AgeTo = w.AgeTo,
               CourcesTakenPrecentage=(int) GetCourcesTaken(w,model.UserId),
                Courses = w.CourseCategories.Select(c => new RoadMapCoursesDTO()
                {
                    group_id = w.Id,
                    Id = c.Id,
                    Title =model.languageId==0? c.Name:c.ArName
                }).ToList()
            }).ToList();
            return Ok(response);
        }
        internal static double GetCourcesTaken(Stage w, int? userId)
        {
            double CourseCount = w.CourseCategories.SelectMany(w => w.Courses).Count();
            double userCourseCount = w.CourseCategories.SelectMany(w => w.Courses).SelectMany(w => w.ChildCourses).Where(w => w.ChildId == userId).DistinctBy(w => w.CourseId).Count();
            return CourseCount==0?0: (userCourseCount / CourseCount) * 100;
        }
    }
}
