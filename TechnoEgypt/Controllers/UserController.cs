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
                Group_Name = stage?.Name,
                Certificates = child.ChildCertificates.Select(w => new Certificat() { Id = w.Id, Image_Url = w.FileURL }).ToList(),
                school = child.SchoolName,
                childern = data.FirstOrDefault().Children.Where(w => w.IsActive).Select(w => w.Id).ToList()
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
                Group_Name = stage?.Name,
                Certificates = child.ChildCertificates.Select(w => new Certificat() { Id = w.Id, Image_Url = w.FileURL }).ToList(),
                school = child.SchoolName,
                childern = _dBContext.children.Where(w => w.ParentId == child.ParentId && w.IsActive).Select(w => w.Id).ToList()

            };
            response.Message = "success";
            return Ok(response);
        }
        [HttpPost("StationData")]
        public IActionResult StationData()
        {
            var response = new Response<List<StationDataDto>>();
            response.Message = "success";
            response.StatusCode = ResponseCode.success;
            var data = _dBContext.station.Include(w => w.childCVDatas).ToList();
            response.Data = data.Select(w => new StationDataDto() { Id = w.Id, Title = w.Title, Desc = w.Description, available = w.IsAvilable }).ToList();
            return Ok(response);
        }
        [HttpPost("UpdateChildCVData")]
        public IActionResult UpdateChildCVData([FromForm] ChildCVoSaveDto model)
        {
            var FilePath = "\\Files\\" + DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Millisecond.ToString() + model.File.FileName;
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
                    Date = DateTime.Now,
                    ChildId = model.UserId.Value,
                    Name = model.Name,
                    FileURL = FilePath,
                    Note=model.Note
                };
                _dBContext.childCVData.Add(station);
            }
            else
            {
                station.Date = DateTime.Now;
                station.Name = model.Name;
                station.Note=model.Note;
                station.FileURL=FilePath;
                _dBContext.Entry<ChildCVData>(station).State = EntityState.Modified;
            }
            _dBContext.SaveChanges();
            return Ok();
        }
        //[HttpPost("GetFileData")]
        //public IActionResult GetFileData(string FileName)
        //{
        //    var path = env.ContentRootPath + FileName;
        //    byte[] bytes = System.IO.File.ReadAllBytes(path);
        //    String AsBase64String = Convert.ToBase64String(bytes);

        //    return Ok(AsBase64String);
        //}
    }
}
