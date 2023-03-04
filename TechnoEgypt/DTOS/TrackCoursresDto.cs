namespace TechnoEgypt.DTOS
{
    public class TrackCoursresDto
    {
        public int Id { get; set; }
        public int Track_Id { get; set; }
        public string Course_Title { get; set; }
        public string Tool { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? ValidFrom { get; set; }
        public bool IsAvailable { get; set; }
    

    }
}
