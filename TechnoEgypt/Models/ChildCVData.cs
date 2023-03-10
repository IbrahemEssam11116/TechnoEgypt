using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
  public enum StationType
    {
        Awards=0,
        championships,
        Others,
        LanguageTests,
        Volunteer,
        Internship,
        HighSchool,
        InternationalTest,
        RecommendationLetters,
        
    }
    public class ChildCVData
    {
        [Key]
        public int Id { get; set; }
        public string Name{ get; set; }
        public DateTime Date { get; set; }
        public string FileURL { get; set; }
        public string Note { get; set; }
        public StationType stationId { get; set; }
        //[ForeignKey("stationId")]
        //public station station { get; set; }
        public int ChildId{ get; set; }
        [ForeignKey("ChildId")]
        public child Child{ get; set; }
    }
}
