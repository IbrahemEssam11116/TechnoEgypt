using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;

namespace TechnoEgypt.Areas.Identity.Data;

public class UserDbContext : IdentityDbContext<AppUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<child> children { get; set; }
    public DbSet<ChildCourse> childCourses { get; set; }
    public DbSet<ChildCVData> childCVData { get; set; }
    public DbSet<ChildSchoolReport> childSchoolReports { get; set; }
    public DbSet<ChildPersonalStatement> childPersonalStatements { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseCategory> CourseCategories { get; set; }
    public DbSet<CourseTool> CourseToolsMyProperty { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<ChildCertificate> ChildCertificates { get; set; }
    //public DbSet<station> station { get; set; }
    public DbSet<Branch> Branch { get; set; }
    public DbSet<ParentMessage> ChildMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
