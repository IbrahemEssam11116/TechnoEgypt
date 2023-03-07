using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
	public class WebCourseIndex
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Course course { get; set; }

		[DisplayName("Tool Name")]
		public int ToolID { get; set; }
		public string ToolName { get; set; }
		public List<CourseTool> Tools { get; set; }
		public SelectList ToolList { get; set; }

		[DisplayName("Stage Name")]
		public int StageId { get; set; }
		public string StageName { get; set; }
		public List<Stage> Stages { get; set; }
		public SelectList StageList { get; set; }

		[DisplayName("Track Name")]
		public int CourseCategoryId { get; set; }
		public string CourseCategoryName { get; set; }
		public List<CourseCategory> CourseCategories { get; set; }
		public SelectList CourseCategoryList { get; set; }

	}
}
