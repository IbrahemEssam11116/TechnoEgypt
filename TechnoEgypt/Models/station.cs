using System.ComponentModel.DataAnnotations;

namespace TechnoEgypt.Models
{
    public class station
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ArTitle { get; set; }
        public string ArDescription { get; set; }
        public string IconURL { get; set; }
        public bool IsAvilable { get; set; }
        public ICollection<ChildCVData> childCVDatas { get; set; }
    }
}
