using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int StageId { get; set; }
        [ForeignKey("StageId")]
        public Stage Stage { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
