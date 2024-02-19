using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnoEgypt.Models
{
    public enum StationType
    {
        IqTest = 1,
        Awards = 2,
        Others = 3,
        LanguageTests = 4,
        Volunteer = 5,
        Internship = 6,
        HighSchool = 7,
        InternationalTest = 8,
        RecommendationLetters = 9,
        otherActivity = 10,
        BirthDate = 11,
        PassPort = 12,
        otherPrograms = 13

    }
    public class ChildCVData
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string FileURL { get; set; }
        public string Note { get; set; }
        public StationType stationId { get; set; }
        //[ForeignKey("stationId")]
        //public station station { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("ChildId")]
        public child Child { get; set; }
    }
}
