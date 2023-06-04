using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
    public class Student
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int ParentId { get; set; }
        public Parents parent { get; set; }
        public  child childrens { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<ChildCVData> ChildCVs { get; set; }
        public ICollection<ChildCourse> ChildCourses { get; set; }
        public ICollection<ChildCertificate> ChildCertificates { get; set; }


    }
}
