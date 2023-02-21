using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.Models
{
    public class Stage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public ICollection<CourseCategory>CourseCategories { get; set; }
    }
}
