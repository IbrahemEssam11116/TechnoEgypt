using System.ComponentModel;

namespace TechnoEgypt.ViewModel
{
    public class WebMessagesIndex
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        [DisplayName("Sender Name")]
        public int SenderID { get; set; }
        public string SenderName { get; set; }
        public string Date { get; set; }

    }
}
