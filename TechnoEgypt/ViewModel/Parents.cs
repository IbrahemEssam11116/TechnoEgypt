using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechnoEgypt.ViewModel
{
    public class Parents
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public Parents parent { get; set; }
        public string UserName { get; set; }
        public int StudentCount { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string MotherPhoneNumber { get; set; }
        public string FatherTitle { get; set; }
        public string MotherTitle { get; set; }

        //[DisplayName("Tool Name")]
        //      public int ToolID { get; set; }
        //      public string ToolName { get; set; }
        //      public IFormFile Image { get; set; }
        //public List<child> children { get; set; }
        public SelectList children { get; set; }

        //[DisplayName("Stage Name")]
        //public int StageId { get; set; }
        //public string StageName { get; set; }
        //public List<Stage> Stages { get; set; }
        //public SelectList StageList { get; set; }

        //[DisplayName("Track Name")]
        //public int CourseCategoryId { get; set; }
        //public string CourseCategoryName { get; set; }
        //public List<CourseCategory> CourseCategories { get; set; }
        //public SelectList CourseCategoryList { get; set; }

    }
}
