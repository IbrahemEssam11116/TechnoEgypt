using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TechnoEgypt.Models;

namespace TechnoEgypt.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public int? BranchId { get; set; }
    [ForeignKey("BranchId")]
    public Branch Branc { get; set; }
}

