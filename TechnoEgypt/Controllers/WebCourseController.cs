using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
	public class WebCourseController : Controller
	{
		private readonly AppDBContext _dBContext;
		public WebCourseController(AppDBContext dBContext)
		{
			_dBContext = dBContext;
		}
		public IActionResult Index()
		{

			var courses = _dBContext.Courses
				.Include(Courses => Courses.CourseCategory).ThenInclude(CourseCategory => CourseCategory.Stage)
				.Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, CourseCategoryName = w.CourseCategory.Name, StageName= w.CourseCategory.Stage.Name })
				.ToList();

			return View(courses);
		}
		public async Task<IActionResult> AddOrEdit(int? Id)
		{
            WebCourseIndex courses = new WebCourseIndex();
            courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
            courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);
			

            ViewBag.PageName = Id == null ? "Create Course" : "Edit Course";
			ViewBag.IsEdit = Id == null ? false : true;
			if (Id == null)
			{
				
                return View(courses);
			}
			else
			{
				 courses = await _dBContext.Courses.Where(w => w.Id == Id)
					.Include(Courses => Courses.CourseCategory).ThenInclude(CourseCategory => CourseCategory.Stage).
					Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, StageId = w.CourseCategory.StageId , CourseCategoryId= w.CourseCategoryId })
					.FirstOrDefaultAsync();
				courses.Stages = _dBContext.Stages.ToList();
				courses.CourseCategories = _dBContext.CourseCategories.ToList();
				courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
				courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);

				//courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", coursecategory.StageId);

				if (courses == null)
				{
					return NotFound();
				}
				return View(courses);
			}

		}
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> AddOrEdit(CourseCategoryIndex coursecategory)
		//{

		//	// coursecategory.Stages = _dBContext.Stages.ToList();


		//	bool IsEmployeeExist = false;

		//	var coursecategoryData = await _dBContext.CourseCategories.FindAsync(coursecategory.Id);


		//	if (coursecategoryData != null)
		//	{
		//		IsEmployeeExist = true;
		//	}
		//	else
		//	{
		//		coursecategoryData = new CourseCategory();
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		try
		//		{
		//			coursecategoryData.Name = coursecategory.Name;
		//			coursecategoryData.StageId = coursecategory.StageId;
		//			// stageData.AgeTo = stage.AgeTo;
		//			if (IsEmployeeExist)
		//			{
		//				_dBContext.Update(coursecategoryData);
		//			}
		//			else
		//			{
		//				_dBContext.CourseCategories.Add(coursecategoryData);
		//			}
		//			await _dBContext.SaveChangesAsync();
		//		}
		//		catch (DbUpdateConcurrencyException)
		//		{
		//			throw;
		//		}
		//		return RedirectToAction(nameof(Index));
		//	}
		//	return RedirectToAction("Index", "CourseCategory");
		//}
		//public async Task<ActionResult> Delete(int Id)
		//{
		//	var track = await _dBContext.CourseCategories.FindAsync(Id);
		//	_dBContext.Entry(track).State = EntityState.Deleted;
		//	await _dBContext.SaveChangesAsync();
		//	//AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

		//	return RedirectToAction("Index");
		//}

	}
}