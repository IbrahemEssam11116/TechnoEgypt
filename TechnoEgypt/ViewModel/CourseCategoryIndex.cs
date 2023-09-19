using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
    public class CourseCategoryIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public string ArName { get; set; }

		public string TrackName { get; set; }
        [DisplayName("Stage Name")]
        [Required(ErrorMessage ="aloooooooooooooooooooo")]
        public int StageId { get; set; }
        public List<Stage> Stages { get; set; }
        public SelectList StageList{ get; set; }

    }
}
