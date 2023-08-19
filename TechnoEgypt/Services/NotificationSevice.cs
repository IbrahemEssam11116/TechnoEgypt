using System.Text;
using TechnoEgypt.Areas.Identity.Data;

namespace TechnoEgypt.Services
{
    public class NotificationSevice
    {
        private static readonly HttpClient client = new HttpClient();
        public UserDbContext _context { get; }

        public NotificationSevice(UserDbContext context)
        {
            _context = context;
        }


        public async Task SendNotification(string title,string message, int parentId)
        {
            var deviceToken = _context.Parents.Find(parentId)?.Token;
            if(deviceToken == null)
            {
                return;
            }
            string serverKey = "AAAARBjwleA:APA91bFqFsHnAXIPx4w72zPAYTMvlj3-ZAaQhGkDnqoJA3B0OP-nV3YN1QuDU9wQwL1W37N_XSMt10ggrzS2XRZ_l6ZXtTg7C7g2_g4wJlxJbJll6iySyoEzxqefcVKyCFzJSnGuocpo";
            string url = "https://fcm.googleapis.com/fcm/send";

            var payload = new
            {
                notification = new
                {
                    title = title,
                    body = message
                },
                to = deviceToken
            };

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + serverKey);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            var response = await client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));


        }
    }
}
