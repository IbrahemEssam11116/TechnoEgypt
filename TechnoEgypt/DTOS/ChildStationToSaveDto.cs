using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.DTOS
{
    public class ChildCVoGetDto : BaseDto
    {
        public StationType StationId { get; set; }

    }
    public class ChildCVoSaveDto : BaseDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public StationType? StationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}
