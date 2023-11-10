using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.DTOS
{

    public class ChildSchoolData : BaseDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string SchoolName { get; set; }
        public double Grade { get; set; }
        public string Note { get; set; }
    }
}
