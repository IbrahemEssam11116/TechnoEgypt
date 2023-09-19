using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
    public class StaffController : Controller
    {
        private readonly UserDbContext _dBContext;
        private readonly IWebHostEnvironment env;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public StaffController(UserDbContext dBContext, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _dBContext = dBContext;
            this.env = env;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            var users = _dBContext.Users
                .Include(User => User.Branc)
                .Select(w => new Staffindex { Id = w.Id, UserName = w.UserName, Email = w.Email, Phone = w.PhoneNumber, BranchId = w.BranchId, BranchName = w.Branc.Name })
                .ToList();
            //var courses = _dBContext.Courses
            //    .Include(Courses => Courses.CourseCategory).ThenInclude(CourseCategory => CourseCategory.Stage)
            //    .Include(courses => courses.courseTool)
            //    .Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, CourseCategoryName = w.CourseCategory.Name, StageName = w.CourseCategory.Stage.Name, ToolName = w.courseTool.Name })
            //    .ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddBranch(string userId)
        {
            ViewBag.Branches = new SelectList(_dBContext.Branch.ToList(), "Id", "Name");
            return PartialView(new UserBranch() { UserId = userId });
        }
        [HttpGet]
        public async Task<IActionResult> AddPermission(string userId)
        {
            ViewBag.permissions = new SelectList(_dBContext.Roles.ToList(), "NormalizedName", "Name");
            var permissions = _dBContext.UserRoles.Where(w => w.UserId == userId).Select(w => w.RoleId).ToList();
            List<string> namesPermissions = new List<string>();
            foreach (var item in permissions)
            {
                try
                {
                    var a = await _roleManager.FindByIdAsync(item);
                    namesPermissions.Add(a.NormalizedName);
                }
                catch (Exception ex)
                {

                }
              
            }
            return PartialView(new UserPermissions() { UserId = userId, PermissionIds = namesPermissions });
        }
        public async Task<IActionResult> AddPermission(UserPermissions model)
        {
            var user = _dBContext.Users.Find(model.UserId);
            _dBContext.UserRoles.RemoveRange(_dBContext.UserRoles.Where(w => w.UserId == model.UserId));
            foreach (var item in model.PermissionIds)
            {
              var a=  await _userManager.AddToRoleAsync(user, item);
            }
            return RedirectToAction("Index", "Staff");

        }
        public IActionResult AddBranch(UserBranch model)
        {

            var user = _dBContext.Users.Find(model.UserId);
            if (model.branchId == 0)
            {
                model.branchId = null;
            }
            user.BranchId = model.branchId;
            _dBContext.SaveChanges();
            return RedirectToAction("Index", "Staff");
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
                .Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, StageId = w.CourseCategory.Stage.Id, StageName = w.CourseCategory.Stage.Name, CourseCategoryId = w.CourseCategoryId, CourseCategoryName = w.CourseCategory.Name, ToolID = w.ToolId, ToolName = w.courseTool.Name })
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

        public IActionResult createCertificate()
        {
            System.Drawing.Point point;
            var userc = 1;
            var pathin = env.WebRootPath + "\\Files\\certificate-2023-1.pdf";
            //MedLineUserCourseEntity userCourseEntity = new MedLineUserCourseEntity(UserCourseID);
            //PrefetchPath2 path = new PrefetchPath2(EntityType.MedLineUserCourseEntity);
            //path.Add(MedLineUserCourseEntity.PrefetchPathMedLineCourseItem);
            //path.Add(MedLineUserCourseEntity.PrefetchPathMedLineUserItem);
            //EMedLine.BL.DataBaseClassHelper.FillEntity(userCourseEntity, path);

            //string oldFile = "E:\\EMDEDLINE_Files\\EMDEDLINE_Files\\CertificateTemp.pdf";
            //string newFile = "E:\\EMDEDLINE_Files\\EMDEDLINE_Files\\CertificateTemp1.pdf";
            //string pathin = "E:\\EMDEDLINE_Files\\EMDEDLINE_Files\\CertificateTemp.pdf";
            //var pathin = Path.Combine(env.WebRootPath, "CertificateTemp.pdf");
            var pathfont = Path.Combine(env.WebRootPath, "MTCORSVA.TTF");
            //string newName = $"{Guid.NewGuid():N}.pdf";
            ////string FolderPath = $"{MainFolderPath}/{userCourseEntity.LearnerID}";
            //if (!Directory.Exists(FolderPath))
            //    Directory.CreateDirectory(FolderPath);
            //string pathout = $"{FolderPath}/{newName}";
            string pathout = env.WebRootPath + "\\Files\\new.pdf";
            //userCourseEntity.Certificate = Path.Combine(userCourseEntity.LearnerID.ToString(), newName);

            //create PdfReader object to read from the existing document
            using (PdfReader reader = new PdfReader(pathin))
            //create PdfStamper object to write to get the pages from reader 
            using (PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout, FileMode.Create)))
            {
                //select two pages from the original document
                reader.SelectPages("1-2");

                //gettins the page size in order to substract from the iTextSharp coordinates
                var pageSize = reader.GetPageSize(1);

                // PdfContentByte from stamper to add content to the pages over the original content
                PdfContentByte pbover = stamper.GetOverContent(1);
                BaseFont bf = BaseFont.CreateFont(@pathfont, "Identity-H", BaseFont.EMBEDDED);
                Font NameFont = new Font(bf, 10);
                NameFont.Size = 26;

                //add content to the page using ColumnText
                Font font = new Font();
                font.Size = 18;
                string UserName = userCourseEntity.MedLineUserItem.Fname + userCourseEntity.MedLineUserItem.Mname + userCourseEntity.MedLineUserItem.Lname;
                string CourseName = userCourseEntity.MedLineCourseItem.Title;
                //font.Style = "Arial";
                //setting up the X and Y coordinates of the document
                point = new System.Drawing.Point(); 
                int x = point.X + 370;
                int y = point.Y + 230;
                int x1 = point.X + 450;
                int y1 = point.Y + 300;
                y = (int)(pageSize.Height - y);
                DateTime now = DateTime.Now;
                string DT = now.ToString().Substring(0, 10);
                int x2 = point.X + 190;
                int y2 = point.Y + 120;

                ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(UserName, NameFont), x, y, 0);
                ColumnText.ShowTextAligned(pbover, Element.ALIGN_CENTER, new Phrase(CourseName, font), x1, y1, 0);
                font.Size = 14;
                ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(DT, font), x2, y2, 0);
                ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(UserCourseID.ToString(), font), 170, 105, 0);


            }
            RelationPredicateBucket filter = new RelationPredicateBucket();
            filter.PredicateExpression.Add(MedLineUserCourseFields.ID == userCourseEntity.ID);
            SStorm.EMedLine.BL.DataBaseClassHelper.UpdateEntityDirectly(userCourseEntity, filter, 0);




            return File(new MemoryStream(),"png");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(WebCourseIndex webcourse)
        {

            // coursecategory.Stages = _dBContext.Stages.ToList();
            string IImageName = "";
            if (webcourse.Image != null)
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
                    courseData.Name = webcourse.Name;
                    courseData.ToolId = webcourse.ToolID;
                    courseData.CourseCategoryId = webcourse.CourseCategoryId;
                    courseData.ImageURL = IImageName;
                    courseData.ArDescripttion = webcourse.course.ArDescripttion;
                    courseData.ArName = webcourse.course.ArName;



                    if (IsEmployeeExist)
                    {
                        _dBContext.Entry<Course>(courseData).State = EntityState.Modified;
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
        public IActionResult GetSpec(int Id)
        {
            var sta = new SelectList(_dBContext.CourseCategories.Where(w => w.StageId == Id).ToList(), "Id", "Name");
            return Json(sta);

        }
        public IActionResult BranchIndex()
        {

            var branchs = _dBContext.Branch.ToList();
            return View(branchs);
        }
        public async Task<IActionResult> BranchAddOrEdit(int? Id)
        {
            ViewBag.PageName = Id == null ? "Create Branch" : "Edit Branch";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var branch = await _dBContext.Branch.FindAsync(Id);

                if (branch == null)
                {
                    return NotFound();
                }
                return View(branch);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BranchAddOrEdit(Branch branch)
        {
            bool IsEmployeeExist = false;

            var branchData = await _dBContext.Branch.FindAsync(branch.Id);

            if (branchData != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                branchData = new Branch();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    branchData.Name = branch.Name;
                    branchData.PhoneNumbers = branch.PhoneNumbers;
                    branchData.WhatsappNumber = branch.WhatsappNumber;
                    branchData.Address = branch.Address;

                    if (IsEmployeeExist)
                    {
                        _dBContext.Update(branchData);
                    }
                    else
                    {
                        _dBContext.Branch.Add(branchData);
                    }
                    await _dBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(BranchIndex));
            }
            return RedirectToAction("BranchIndex", "Staff");
        }
        public async Task<ActionResult> BranchDelete(int Id)
        {
            var branch = await _dBContext.Branch.FindAsync(Id);
            _dBContext.Entry(branch).State = EntityState.Deleted;
            await _dBContext.SaveChangesAsync();
            //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

            return RedirectToAction("BranchIndex");
        }



    }
}