using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.Models
{
    public class CourseTool
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
