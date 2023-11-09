using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class child
    {
        [Key]
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int ParentId { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ParentId")]
        public Parent parent { get; set; }
        public ICollection<ChildCVData> ChildCVs { get; set; }
        public ICollection<ChildCourse> ChildCourses { get; set; }
        public ICollection<ChildCertificate> ChildCertificates { get; set; }


    }
}
