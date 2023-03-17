using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
	public class WebCourseController : Controller
	{
		private readonly UserDbContext _dBContext;
        private readonly IWebHostEnvironment env;

		public WebCourseController(UserDbContext dBContext, IWebHostEnvironment env)
		{
			_dBContext = dBContext;
			this.env = env;
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
            ViewBag.PageName = Id == null ? "Create Course" : "Edit Course";
			ViewBag.IsEdit = Id == null ? false : true;
			if (Id == null)
			{

                courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
                courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);
                courses.ToolList = new SelectList(_dBContext.CourseToolsMyProperty.ToList(), "Id", "Name", courses.ToolID);
                return View(courses);
			}
			else
			{
				
				courses = await _dBContext.Courses.Where(w => w.Id == Id)
				   .Include(Courses => Courses.courseTool).Include(Courses => Courses.CourseCategory).ThenInclude(CourseCategory => CourseCategory.Stage)
				.Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, StageId = w.CourseCategory.Stage.Id, StageName =w.CourseCategory.Stage.Name , CourseCategoryId= w.CourseCategoryId, CourseCategoryName = w.CourseCategory.Name , ToolID = w.ToolId, ToolName = w.courseTool.Name })
				.FirstOrDefaultAsync();
				courses.course = await _dBContext.Courses.Where(w => w.Id == Id).FirstOrDefaultAsync();
				courses.Stages = _dBContext.Stages.ToList();
				courses.CourseCategories = _dBContext.CourseCategories.ToList();
				courses.Tools = _dBContext.CourseToolsMyProperty.ToList();
				courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
				courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);
				courses.ToolList = new SelectList(_dBContext.CourseToolsMyProperty.ToList(), "Id", "Name", courses.ToolID);
				if (courses == null)
				{
					return NotFound();
				}
				return View(courses);
			}

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddOrEdit(WebCourseIndex webcourse)
		{

			// coursecategory.Stages = _dBContext.Stages.ToList();
			string IImageName="";
			if( webcourse.Image != null)
			{
                var FilePath = "Files\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Millisecond.ToString() + webcourse.Image.FileName;
                var path = env.WebRootPath + "\\" + FilePath;
                using (FileStream fs = System.IO.File.Create(path))
                {
                    webcourse.Image.CopyTo(fs);
                }
				IImageName = FilePath;
            }
			bool IsEmployeeExist = false;

			var courseData = await _dBContext.Courses.FindAsync(webcourse.Id);

			
			if (courseData != null)
			{
				IsEmployeeExist = true;
				if (courseData.ImageURL != null && IImageName == "")
				{
					IImageName = courseData.ImageURL;
				}
			}
			else
			{
                courseData = new Course();
			}

			if (ModelState.IsValid)
			{
				try
				{
					//courseData = webcourse.course;
					courseData.Id = webcourse.Id;
					courseData.ToolId = webcourse.ToolID;
					courseData.CourseCategoryId= webcourse.CourseCategoryId;
					courseData.ImageURL = IImageName;
					courseData.ArDescripttion = webcourse.course.ArDescripttion;
					courseData.ArName = webcourse.course.ArName;

					

                    if (IsEmployeeExist)
					{
						_dBContext.Entry<Course>(courseData).State=EntityState.Modified;
					}
					else
					{
						_dBContext.Courses.Add(courseData);
					}
					await _dBContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction("Index", "WebCourse");
		}
		public async Task<ActionResult> Delete(int Id)
		{
			var course = await _dBContext.Courses.FindAsync(Id);
			_dBContext.Entry(course).State = EntityState.Deleted;
			await _dBContext.SaveChangesAsync();
			//AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

			return RedirectToAction("Index");
		}
		//public IActionResult GetFilteredStages(int CourseTypeID)
		//{
		//	RelationPredicateBucket filter = new RelationPredicateBucket();
		//	filter.PredicateExpression.Add(MedLineSpecialityFields.CourseTypeID == CourseTypeID);
		//	var specialityList = DataHelper.GetSelectList<MedLineSpecialityEntity>(MedLineSpecialityFields.ID, MedLineSpecialityFields.Name, filter, null);
		//	return Json(specialityList);
		//}
		//public IActionResult GetFilteredCourseCategory(int CourseTypeID)
		//{
		//	RelationPredicateBucket filter = new RelationPredicateBucket();
		//	filter.PredicateExpression.Add(MedLineSpecialityFields.CourseTypeID == CourseTypeID);
		//	var specialityList = DataHelper.GetSelectList<MedLineSpecialityEntity>(MedLineSpecialityFields.ID, MedLineSpecialityFields.Name, filter, null);
		//	return Json(specialityList);
		//}



	}
}