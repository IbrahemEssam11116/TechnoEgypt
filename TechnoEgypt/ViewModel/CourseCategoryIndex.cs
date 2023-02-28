using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
    public class CourseCategoryIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TrackName { get; set; }

        public List<Stage> Stages { get; set; }

    }
}
