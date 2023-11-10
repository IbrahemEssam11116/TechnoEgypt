using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechnoEgypt.Areas.Identity.Data;

namespace TechnoEgypt.Models
{
    public class ParentMessage
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Parent Parent { get; set; }
        public bool Isread { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string CreatedUserId { get; set; }
        [ForeignKey("CreatedUserId")]
        public AppUser CreatedUser { get; set; }

    }
}

