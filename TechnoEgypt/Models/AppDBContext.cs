using Microsoft.EntityFrameworkCore;

namespace TechnoEgypt.Models
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<child> children { get; set; }
        public DbSet<ChildCourse> childCourses{ get; set; }
        public DbSet<ChildCVData> childCVData { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories{ get; set; }
        public DbSet<CourseTool> CourseToolsMyProperty { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<ChildCertificate> ChildCertificates { get; set; }
        public DbSet<station> station { get; set; }
        public DbSet<Branch> Branch { get; set; }
    }
}
