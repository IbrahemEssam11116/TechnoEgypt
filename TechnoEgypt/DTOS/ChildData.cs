namespace TechnoEgypt.DTOS
{
    public class ChildData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string SchoolName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int ParentId { get; set; }
    }
}
