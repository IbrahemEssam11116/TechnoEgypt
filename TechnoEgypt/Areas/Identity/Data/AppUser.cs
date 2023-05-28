using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TechnoEgypt.Models;

namespace TechnoEgypt.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public int BranchId { get; set; }
    [ForeignKey("BranchId")]
    public Branch Branc { get; set; }
}

