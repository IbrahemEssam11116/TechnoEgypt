namespace TechnoEgypt.DTOS
{
    public class LoginDto:languageDto
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FCMToken { get; set; }
    }
}
