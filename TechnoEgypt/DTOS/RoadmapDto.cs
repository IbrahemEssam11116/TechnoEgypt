namespace TechnoEgypt.DTOS
{
    public class RoadMapCoursesDTO
    {
        public int Id { get; set; }
        public int group_id { get; set; }
        public string Title { get; set; }
      
    }
    public class RoadmapDto
    {
        public string Name{ get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public int CourcesTakenPrecentage { get; set; }
        public int CourseTaken { get; set; }
        public int TrackTotal { get; set; }
        public List<RoadMapCoursesDTO> Courses{ get; set; }
    }
}
