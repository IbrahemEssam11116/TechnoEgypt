using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.DTOS
{

    public class ChildPersonalData : BaseDto
    {
        [Required]
        public string Note { get; set; }
    }
}
