using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;
namespace TechnoEgypt.Controllers
{
    public class StudentController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserDbContext _dBContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment env;

        public StudentController(UserDbContext dBContext, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _dBContext = dBContext;
            _userManager = userManager;
            this.env = env;
        }
        public IActionResult AddStudentCourses(int StudentId)
        {
            ViewBag.Coursers = new SelectList(_dBContext.Courses.ToList(), "Id", "Name");
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
                 .Select(w => new Parents { Id = w.ParentId, StudentId = w.Id, Name = w.Name, FatherTitle = w.parent.FatherTitle, FatherPhoneNumber = w.parent.FatherPhoneNumber, MotherPhoneNumber = w.parent.MotherPhoneNumber })
                 .ToListAsync();
            var parents = await _dBContext.Parents
                .Include(parents => parents.Children)
                .Where(w => w.Children.Count() == 0)
                .Select(w => new Parents { Id = w.Id, FatherTitle = w.FatherTitle, FatherPhoneNumber = w.FatherPhoneNumber, MotherPhoneNumber = w.MotherPhoneNumber })
                .ToListAsync();
            parents.AddRange(student);
            return View(parents);
        }
        public async Task<IActionResult> StudentIndex(int Id)
        {
            var father = await _dBContext.Parents.FindAsync(Id);
            ViewBag.PageName = father.FatherTitle;
            ViewBag.ParentID = Id;
            var student = await _dBContext.children
                 .Select(w => new Parents { Id = w.ParentId, StudentId = w.Id, Name = w.Name, FatherTitle = w.parent.FatherTitle })
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
                var parent = await _dBContext.Parents.Include(w => w.Children).FirstOrDefaultAsync(w => w.Id == Id);

                if (parent == null)
                {
                    return NotFound();
                }
                return View(parent);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Models.Parent parent)
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
                parentData = new Models.Parent();
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
                return RedirectToAction(nameof(AddOrEdit), new { Id = parentData.Id });
            }
            return RedirectToAction("Index", "Student", new { Id = parentData.Id });
        }
        public async Task<IActionResult> AddOrEditStudent(int? Id, int ParentID)
        {
            ViewBag.PageName = Id == null ? "Create Student" : "Edit Student";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                var student = new Student();
                student.ParentId = ParentID;
                return View(student);
            }
            else
            {
                var student = await _dBContext.children.Include(w => w.ChildCourses).ThenInclude(w => w.Course).Select(w => new Student { Id = w.Id, ParentId = w.ParentId, Name = w.Name, SchoolName = w.SchoolName, DateOfBirth = w.DateOfBirth, Phone = w.Phone, ChildCourses = w.ChildCourses }).FirstOrDefaultAsync(w => w.Id == Id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditStudent(Student student)
        {
            string IImageName = "";
            if (student.Image != null)
            {
                var FilePath = "Files\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Millisecond.ToString() + student.Image.FileName;
                var path = env.WebRootPath + "\\" + FilePath;
                using (FileStream fs = System.IO.File.Create(path))
                {
                    student.Image.CopyTo(fs);
                }
                IImageName = FilePath;
            }
            bool IsEmployeeExist = false;

            var studentData = await _dBContext.children.FindAsync(student.Id);

            if (studentData != null)
            {
                IsEmployeeExist = true;
                if (studentData.ImageURL != null && IImageName == "")
                {
                    IImageName = studentData.ImageURL;
                }
            }
            else
            {
                studentData = new child();
                studentData.IsActive = true;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    studentData.Name = student.Name;
                    studentData.ImageURL = IImageName;
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
                return RedirectToAction(nameof(AddOrEdit), new { Id = student.ParentId });
            }
            return RedirectToAction("AddOrEdit", "Student", new { Id = student.ParentId });
        }
        public async Task<ActionResult> DeleteStudent(int Id)
        {
            var student = await _dBContext.children.FindAsync(Id);
            _dBContext.Entry(student).State = EntityState.Deleted;
            await _dBContext.SaveChangesAsync();
            //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int Id)
        {
            var father = await _dBContext.Parents.FindAsync(Id);
            _dBContext.Entry(father).State = EntityState.Deleted;
            await _dBContext.SaveChangesAsync();
            //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> DeleteChildCourse(int Id)
        {
            var studentcourse = await _dBContext.childCourses.FindAsync(Id);
            _dBContext.Entry(studentcourse).State = EntityState.Deleted;
            await _dBContext.SaveChangesAsync();
            //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

            return RedirectToAction("AddOrEditStudent", "Student", new { id = studentcourse.ChildId });
        }
    }
}
