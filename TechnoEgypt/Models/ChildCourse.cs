using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class ChildCourse
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime CertificationDate { get; set; }

        public int ChildId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("ChildId")]
        public child Child{ get; set; }
        public ChildCourseStatus Status{ get; set; }
        public ICollection<ChildCertificate> ChildCertificates { get; set; }

    }
}
