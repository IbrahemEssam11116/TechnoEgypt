using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechnoEgypt.Areas.Identity.Data;

namespace TechnoEgypt.Models
{
    public class Parent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string HomePhoneNumber { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string FatherEmail{ get; set; }
        public string MotherPhoneNumber { get; set; }
        public string MotherEmail { get; set; }
        public string FatherTitle{ get; set; }
        public string MotherTitle{ get; set; }
        [Required]
        public string UserName{ get; set; }
        public string Address{ get; set; }
        public int MyProperty { get; set; }
        public bool IsActive{ get; set; }
        public string Token { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
        public ICollection<child> Children{ get; set; }
    }
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumbers { get; set; }
        public string WhatsappNumber{ get; set; }
        public string Address { get; set; }
        public ICollection<Parent> Parents{ get; set; }
        public ICollection<AppUser> AppUsers{ get; set; }

    }
}
