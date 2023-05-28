using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Migrations;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;
namespace TechnoEgypt.Controllers
{
    public class StudentController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserDbContext _dBContext;
        private readonly UserManager<AppUser> _userManager;

        public StudentController(UserDbContext dBContext, UserManager<AppUser> userManager)
        {
            _dBContext = dBContext;
            _userManager = userManager;
        }
		public IActionResult AddStudentCourses(int StudentId)
        {
			ViewBag.Coursers = new SelectList(_dBContext.Courses.ToList(),"Id","Name");
			return PartialView(new ChildCourse() { ChildId = StudentId });
        }
        [HttpPost]
		public IActionResult SaveStudentCourse(ChildCourse childCourse)
        {
			_dBContext.childCourses.Add(childCourse);
			_dBContext.SaveChanges();
			return RedirectToAction("AddOrEditStudent", "Student", new { Id = childCourse.ChildId });
        }
        public async Task<IActionResult> Index()
        {

            //var parent = _dBContext.Parents
            //    .Include(parents => parents.Children)
            //    .Select(w => new Student { Id = w.Id, StudentCount = w.Children.Count, UserName = w.UserName, FatherTitle = w.FatherTitle, FatherPhoneNumber = w.FatherPhoneNumber })
            //    .ToList();
            //return View(parent);
            var student = await _dBContext.children
                 .Select(w => new Student { Id = w.Id, FatherId =w.ParentId, Name = w.Name, FatherTitle = w.parent.FatherTitle })
                 .ToListAsync();
			return View(student);
        }
        public async Task<IActionResult> StudentIndex(int Id)
        {
            var father = await _dBContext.Parents.FindAsync(Id);
            ViewBag.PageName = father.FatherTitle;
			ViewBag.ParentID = Id;
            var student = await _dBContext.children
                 .Select(w => new Student { Id = w.Id, FatherId = Id,  Name = w.Name , FatherTitle= w.parent.FatherTitle})
                 .ToListAsync();
            return View(student);
        }
        public async Task<IActionResult> AddOrEdit(int? Id)
        {
            ViewBag.PageName = Id == null ? "Create Parent" : "Edit Parent";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var parent = await _dBContext.Parents.FindAsync(Id);

                if (parent == null)
                {
                    return NotFound();
                }
                return View(parent);
            }

        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddOrEdit(Parent parent)
		{
			bool IsEmployeeExist = false;

			var parentData = await _dBContext.Parents.FindAsync(parent.Id);
			var userId = this.User.Identity.GetUserId();
			var branchId = _dBContext.Users.Find(userId).BranchId;

			if (parentData != null)
			{
				IsEmployeeExist = true;
			}
			else
			{
				parentData = new Parent();
				parentData.BranchId = branchId;
			}

			if (ModelState.IsValid)
			{
				try
				{
					parentData.UserName = parent.UserName;
					parentData.HomePhoneNumber = parent.HomePhoneNumber;
					parentData.Address = parent.Address;
					parentData.FatherTitle = parent.FatherTitle;
					parentData.FatherPhoneNumber = parent.FatherPhoneNumber;
					parentData.FatherEmail = parent.FatherEmail;
					parentData.MotherTitle = parent.MotherTitle;
					parentData.MotherPhoneNumber = parent.MotherPhoneNumber;
					parentData.MotherEmail = parent.MotherEmail;
					parentData.BranchId = branchId;
					if (IsEmployeeExist)
					{
						_dBContext.Update(parentData);
					}
					else
					{
						_dBContext.Parents.Add(parentData);
					}
					await _dBContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction("Index", "Student");
		}
		public async Task<IActionResult> AddOrEditStudent(int? Id,int ParentID)
		{
			ViewBag.PageName = Id == null ? "Create Student" : "Edit Student";
			ViewBag.IsEdit = Id == null ? false : true;
			
			if (Id == null)
			{
				var student = new child();
				student.ParentId = ParentID;
				return View(student);
			}
			else
			{
				var student =  _dBContext.children.Include(w=>w.ChildCourses).ThenInclude(w=>w.Course).FirstOrDefault(w=>w.Id==Id);

				if (student == null)
				{
					return NotFound();
				}
				return View(student);
			}

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddOrEditStudent(child student)
		{
			bool IsEmployeeExist = false;

			var studentData = await _dBContext.children.FindAsync(student.Id);

			if (studentData != null)
			{
				IsEmployeeExist = true;
			}
			else
			{
				studentData = new child();
			}

			if (ModelState.IsValid)
			{
				try
				{
					studentData.Name = student.Name;
					studentData.ImageURL = student.ImageURL;
					studentData.SchoolName = student.SchoolName;
					studentData.DateOfBirth = student.DateOfBirth;
					studentData.Phone = student.Phone;
					studentData.ParentId = student.ParentId;
					
					if (IsEmployeeExist)
					{
						_dBContext.Update(studentData);
					}
					else
					{
						_dBContext.children.Add(studentData);
					}
					await _dBContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
				return RedirectToAction(nameof(StudentIndex),new {Id=student.ParentId });
			}
			return RedirectToAction("StudentIndex", "Student", new {Id= student.ParentId});
		}
		//public ActionResult Delete(int ID)
		//{

		//	return PartialView("DeleteItem", new DeleteItemModel
		//	{
		//		ItemID = ID,
		//		ActionName = "Delete",
		//		ControllerName = "StageController",
		//	});
		//}

		////[SmartAuthorize(UserPermission.Course_Delete)]
		//[HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
		//public async Task<ActionResult> ConfirmDelete(int ItemID)
		//{
		//	var stage = await _dBContext.Stages.FindAsync(ItemID);
		//	_dBContext.Entry(stage).State = EntityState.Deleted;
		//	_dBContext.SaveChangesAsync();
		//	//AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

		//	return RedirectToAction("Index");
		//}
		//public async Task<ActionResult> Delete(int Id)
		//{
		//	var stage = await _dBContext.Stages.FindAsync(Id);
		//	_dBContext.Entry(stage).State = EntityState.Deleted;
		//	await _dBContext.SaveChangesAsync();
		//	AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

		//	return RedirectToAction("Index");
		//}
	}
}
