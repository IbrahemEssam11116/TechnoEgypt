using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class ChildCertificate
    {
        [Key]
        public int Id { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("ChildId")]
        public child Child { get; set; }
        public string FileURL { get; set; }
        public int? ChildCourseId { get; set; }
        [ForeignKey("ChildCourseId")]
        public ChildCourse ChildCourse { get; set; }
    }
}
