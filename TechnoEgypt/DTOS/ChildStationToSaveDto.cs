using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.DTOS
{
    public class ChildCVoSaveDto:BaseDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int StationId { get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}
