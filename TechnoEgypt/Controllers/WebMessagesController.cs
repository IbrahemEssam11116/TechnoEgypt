using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.DTOS;
using TechnoEgypt.Migrations;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
    public class WebMessagesController : Controller
    {
        private readonly UserDbContext _dBContext;
        private readonly IWebHostEnvironment env;
		private readonly UserManager<AppUser> _userManager;

		public WebMessagesController(UserDbContext dBContext, UserManager<AppUser> userManager,  IWebHostEnvironment env)
        {
            _dBContext = dBContext;
            this.env = env;
			_userManager = userManager;
			
		}
        public IActionResult Index()
        {

            var Messages = _dBContext.ChildMessages
                .Include(Messages => Messages.Child)
                .Select(w => new WebMessagesIndex { Id = w.Id, Message = w.Message})
                .ToList();

            return View(Messages);


        }
        [HttpGet]
        public IActionResult CreateMessage(string userId)
        {
            ViewBag.Messages = new SelectList(_dBContext.ChildMessages.ToList(), "Id", "Name");
            return PartialView(new WebMessagesIndex() { Message = userId });
        }
        [HttpPost]
        public IActionResult CreateMessage(WebMessagesIndex model)
        {
            var parent = _dBContext.Parents.Select(w=>w.Id).ToList();

			var MessageInfo = new ChildMessage();
            var ParentList = new SelectList(parent,"id");
			var userId = this.User.Identity.GetUserId();

			foreach (var item in parent)
            {
                MessageInfo.ChildId = item;
				MessageInfo.Message = model.Message;
				// MessageInfo.Title = model.Title;
				// MessageInfo.SenderId = userId;
				// MessageInfo.Date = DateTime.Now();
				_dBContext.ChildMessages.Add(MessageInfo);
				
			}
			_dBContext.SaveChanges();
            return RedirectToAction("Index", "WebMessages");
        }
    }
}