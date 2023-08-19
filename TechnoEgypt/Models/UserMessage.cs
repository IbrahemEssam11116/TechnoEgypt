using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class ChildMessage
    {
        [Key]
        public int Id { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("ChildId")]
        public child Child { get; set; }
        public bool Isread{ get; set; }
        public string Message{ get; set; }
    }
}
