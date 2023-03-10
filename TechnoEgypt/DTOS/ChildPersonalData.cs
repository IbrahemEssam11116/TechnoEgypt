using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.DTOS
{
 
    public class ChildPersonalData : BaseDto
    {
        [Required]
        public string Note { get; set; }
    }
}
