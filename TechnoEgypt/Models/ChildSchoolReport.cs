using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{

    public class ChildSchoolReport
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public double Grade { get; set; }
        public DateTime Date { get; set; }
        public string FileURL { get; set; }
        public string Note { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("ChildId")]
        public child Child { get; set; }
    }
}
