namespace TechnoEgypt.DTOS
{
    public enum language
    {
        english = 0, arabic = 1
    }
    public class languageDto
    {
        public language languageId { get; set; }
    }
    public class BaseDto : languageDto
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
    }
}
