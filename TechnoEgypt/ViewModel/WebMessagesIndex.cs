using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
    public class WebMessagesIndex
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        [DisplayName("Sender Name")]
        public int SenderID { get; set; }
        public DateTime Date { get; set; }

    }
}
