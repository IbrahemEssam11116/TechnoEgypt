using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using TechnoEgypt.DTOS;
using TechnoEgypt.Models;

namespace TechnoEgypt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDBContext _dBContext;

        public CourseController(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        [HttpPost("GetCourseById")]
        public IActionResult GetCourseById(BaseDto model)
        {
            var response = new Response<CourseDto>();
            var course=_dBContext.Courses.Where(w=>w.Id== model.Id).Include(w=>w.ChildCourses).FirstOrDefault();
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
                Desc = course.Descripttion,
                Title = course.Name,
                Image_URL = course.ImageURL,
                IsAvailable = !course.ChildCourses.Any(w => w.ChildId == model.UserId)
            };
            return Ok(response);
        }

        [HttpPost("GetTrakeDataById")]
        public IActionResult GetTrakeDataById(BaseDto model)
        {
            var response = new Response<List<TrackCoursresDto>>();
            var courses = _dBContext.Courses.Where(w => w.CourseCategoryId == model.Id).Include(w => w.courseTool);
            response.StatusCode=ResponseCode.success;
            response.Message = "success";
            response.Data = courses.Select(w => new TrackCoursresDto()
            {
                Course_Title=w.Name,
                Id=w.Id,
                Tool=w.courseTool.Name,
                Track_Id=w.CourseCategoryId,
                ValidFrom=w.ValidFrom,
                ValidTo=w.ValidTo
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
            _dBContext.SaveChanges();
            var response = new Response<string>();
            response.StatusCode = ResponseCode.success;
            response.Message= "success";
            response.Data = "success";
            return Ok(response);
        }

        [HttpPost("GetRoadMap")]
        public IActionResult GetRoadMap(languageDto model)
        {
            var response = new Response<List<RoadmapDto>>();
            response.StatusCode= ResponseCode.success;
            response.Message = "success";
            var data = _dBContext.Stages.Include(w => w.CourseCategories);
            response.Data = data.Select(w => new RoadmapDto()
            {
                Name= w.Name,
                Courses= w.CourseCategories.Select(c=>new RoadMapCoursesDTO() 
                { 
                group_id=w.Id,
                Id=c.Id,
                Title=c.Name
                }).ToList()
            }).ToList();
            return Ok(response);
        }
    }
}
