using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;
namespace TechnoEgypt.Controllers
{
	public class CourseToolController : Controller
	{
		private readonly UserDbContext _dBContext;
		public CourseToolController(UserDbContext dBContext)
		{
			_dBContext = dBContext;
		}
		public IActionResult Index()
		{
			var coursetool = _dBContext.CourseToolsMyProperty.ToList();
			return View(coursetool);
		}
		public async Task<IActionResult> AddOrEdit(int? Id)
		{
			ViewBag.PageName = Id == null ? "Create Tool" : "Edit Tool";
			ViewBag.IsEdit = Id == null ? false : true;
			if (Id == null)
			{
				return View();
			}
			else
			{
				var coursetool = await _dBContext.CourseToolsMyProperty.FindAsync(Id);

				if (coursetool == null)
				{
					return NotFound();
				}
				return View(coursetool);
			}

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddOrEdit(CourseTool coursetool)
		{
			bool IsEmployeeExist = false;

			var courseToolData = await _dBContext.CourseToolsMyProperty.FindAsync(coursetool.Id);

			if (courseToolData != null)
			{
				IsEmployeeExist = true;
			}
			else
			{
				courseToolData = new CourseTool();
			}

			if (ModelState.IsValid)
			{
				try
				{
                    courseToolData.Name = coursetool.Name;
					if (IsEmployeeExist)
					{
						_dBContext.Update(courseToolData);
					}
					else
					{
						_dBContext.CourseToolsMyProperty.Add(courseToolData);
					}
					await _dBContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction("Index", "CourseTool");
		}
		public async Task<ActionResult> Delete(int Id)
		{
			var coursetool = await _dBContext.CourseToolsMyProperty.FindAsync(Id);
			_dBContext.Entry(coursetool).State = EntityState.Deleted;
			await _dBContext.SaveChangesAsync();
			//AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

			return RedirectToAction("Index");
		}
	}
}
