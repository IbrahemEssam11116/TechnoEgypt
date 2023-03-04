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
		public string StageName { get; set; }
		
		public string CourseCategoryName { get; set; }
		[DisplayName("Track Name")]
		[Required(ErrorMessage = "aloooooooooooooooooooo")]
		public int StageId { get; set; }
		public int CourseCategoryId { get; set; }
		

		public List<Stage> Stages { get; set; }
		public SelectList StageList { get; set; }

		public List<CourseCategory> CourseCategories { get; set; }
		public SelectList CourseCategoryList { get; set; }

	}
}
