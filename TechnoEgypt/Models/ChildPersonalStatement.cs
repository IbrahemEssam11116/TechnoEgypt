using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{

    public class ChildPersonalStatement
    {
        [Key]
        public int Id { get; set; }
        public string Note { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("ChildId")]
        public child Child { get; set; }
    }
}
