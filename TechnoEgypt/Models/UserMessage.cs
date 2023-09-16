using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechnoEgypt.Areas.Identity.Data;

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
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string CreatedUserId { get; set; }
        [ForeignKey("CreatedUserId")]
        public AppUser CreatedUser { get; set; }

    }
}

