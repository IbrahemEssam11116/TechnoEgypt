using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripttion { get; set; }
        public string ImageURL { get; set; }
        public bool DataCollectionandAnalysis { get; set; }
        public bool CriticalThinking { get; set; }
        public bool Planning { get; set; }
        public bool MathematicalReasoning { get; set; }
        public bool Innovation { get; set; }
        public bool LogicalThinking{ get; set; }
        public bool CognitiveAbilities { get; set; }
        public bool ProblemSolving { get; set; }
        public bool SocialLifeSkills{ get; set; }
        public bool ScientificResearch { get; set; }
        public int CourseCategoryId { get; set; }
        [ForeignKey("CourseCategoryId")]
        public CourseCategory CourseCategory { get; set; }
        public int ToolId { get; set; }
        [ForeignKey("ToolId")]
        public CourseTool courseTool { get; set; }
        public ICollection<ChildCourse> ChildCourses { get; set; }

    }
}
