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
                var a = await _userManager.AddToRoleAsync(user, item);
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
        [HttpGet]
        //public IActionResult createCertificate(int userc)
        //{
        //    System.Drawing.Point point;
        //   // var userc = 1;
        //    var pathin = env.WebRootPath + "\\Files\\blank.pdf";
        //    var usercoursedata = _dBContext.childCourses.Where(w => w.Id == userc).Include(w => w.Course).Include(w => w.Child).FirstOrDefault();
        //    //var pathin = Path.Combine(env.WebRootPath, "CertificateTemp.pdf");
        //    var pathfont = Path.Combine(env.WebRootPath, "MTCORSVA.TTF");
        //    //string newName = $"{Guid.NewGuid():N}.pdf";
        //    ////string FolderPath = $"{MainFolderPath}/{userCourseEntity.LearnerID}";
        //    //if (!Directory.Exists(FolderPath))
        //    //    Directory.CreateDirectory(FolderPath);
        //    //string pathout = $"{FolderPath}/{newName}";
        //    string pathout = env.WebRootPath + "\\Files\\new.pdf";
        //    //userCourseEntity.Certificate = Path.Combine(userCourseEntity.LearnerID.ToString(), newName);

        //    //create PdfReader object to read from the existing document
        //    using (PdfReader reader = new PdfReader(pathin))
        //    //create PdfStamper object to write to get the pages from reader 
        //    using (MemoryStream ms = new MemoryStream())
        //    {

        //        using (PdfStamper stamper = new PdfStamper(reader, ms, '\0',true))
        //        {
        //            //select two pages from the original document
        //            reader.SelectPages("1-2");

        //            //gettins the page size in order to substract from the iTextSharp coordinates
        //            var pageSize = reader.GetPageSize(1);

        //            // PdfContentByte from stamper to add content to the pages over the original content
        //            PdfContentByte pbover = stamper.GetOverContent(1);
        //            //BaseFont bf = BaseFont.CreateFont(@pathfont, "Identity-H", BaseFont.EMBEDDED);
        //            //Font NameFont = new Font(bf, 10);
        //            //NameFont.Size = 26;

        //            //add content to the page using ColumnText
        //            var bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //            Font font = new Font(bf , 50, Font.NORMAL, BaseColor.RED);
        //            //font.Size = 50;
        //            //font.Color = BaseColor.RED;
        //            string UserName = usercoursedata.Child.Name;
        //            string CourseName = usercoursedata.Course.Name;
        //            //font.Style = "Arial";
        //            //setting up the X and Y coordinates of the document
        //            point = new System.Drawing.Point();
        //            int x = point.X + 370;
        //            int y = point.Y + 230;
        //            int x1 = point.X + 450;
        //            int y1 = point.Y + 300;
        //            y = (int)(pageSize.Height - y);
        //            DateTime now = DateTime.Now;
        //            string DT = now.ToString().Substring(0, 10);
        //            int x2 = point.X + 190;
        //            int y2 = point.Y + 120;

        //            ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(UserName, font), 100, 100, 0);
        //            ColumnText.ShowTextAligned(pbover, Element.ALIGN_CENTER, new Phrase(CourseName, font), 100, 100, 0);
        //            font.Size = 14;
        //            ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(DT, font), x2, y2, 0);
        //            ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(userc.ToString(), font), 170, 105, 0);
        //            ms.Position = 0;
        //            var bytes = ms.ToArray();
        //            stamper.Close();
        //            return File(bytes, "application/pdf","a.pdf");

        //        }
        //    }
        //    //RelationPredicateBucket filter = new RelationPredicateBucket();
        //    //filter.PredicateExpression.Add(MedLineUserCourseFields.ID == userCourseEntity.ID);
        //    //SStorm.EMedLine.BL.DataBaseClassHelper.UpdateEntityDirectly(userCourseEntity, filter, 0);




        //}
        
        //public void createCertificatenew(int userc)
        //{
            
        //    var oldFile = env.WebRootPath + "\\Files\\certificate-2023-1.pdf";
        //    string newFile = env.WebRootPath + "\\Files\\new.pdf";

        //    var usercoursedata = _dBContext.childCourses.Where(w => w.Id == userc).Include(w => w.Course).Include(w => w.Child).FirstOrDefault();
        //    string UserName = usercoursedata.Child.Name;
        //    string CourseName = usercoursedata.Course.Name;
        //    // open the reader
        //    PdfReader reader = new PdfReader(oldFile);
        //    Rectangle size = reader.GetPageSizeWithRotation(1);
        //    Document document = new Document(size);

        //    // open the writer
        //    FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();
        //    // the pdf content
        //    PdfContentByte cb = writer.DirectContent;
        //    // select the font properties
        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    cb.SetColorFill(BaseColor.DARK_GRAY);
        //    cb.SetFontAndSize(bf, 50);
        //    // write the text in the pdf content
        //    cb.BeginText();
        //   // string text = "Some random blablablabla...";
        //    // put the alignment and coordinates here
        //    cb.ShowTextAligned(1, UserName, 520, 640, 0);
        //    cb.EndText();
        //    cb.BeginText();
        //   // text = "Other random blabla...";
        //    // put the alignment and coordinates here
        //    cb.ShowTextAligned(2, CourseName, 100, 200, 0);
        //    cb.EndText();
        //    // create the new page and add it to the pdf
        //    PdfImportedPage page = writer.GetImportedPage(reader, 1);
        //    cb.AddTemplate(page, 0, 0);
            
            
        //    // close the streams and voilá the file should be changed :)
        //    document.Close();
        //    fs.Close();
        //    writer.Close();
        //    reader.Close();
        //}
        public void createCertificate(int userc)
        {

            var usercoursedata = _dBContext.childCourses.Where(w => w.Id == userc).Include(w => w.Course).Include(w => w.Child).ThenInclude(w =>w.parent).FirstOrDefault();
            string UserName = usercoursedata.Child.Name +" "+ usercoursedata.Child.parent.FatherTitle;
            string CourseName = usercoursedata.Course.Name;
            var Cdate = usercoursedata.CertificationDate.ToString();
            string oldFile = env.WebRootPath + "\\Files\\certificate-2023-1.pdf";
            string watermarkedFile = env.WebRootPath + "\\Files\\new.pdf";
            // Creating watermark on a separate layer
            // Creating iTextSharp.text.pdf.PdfReader object to read the Existing PDF Document
            PdfReader reader1 = new PdfReader(oldFile);
            using (FileStream fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
            // Creating iTextSharp.text.pdf.PdfStamper object to write Data from iTextSharp.text.pdf.PdfReader object to FileStream object
            using (PdfStamper stamper = new PdfStamper(reader1, fs))
            {
                // Getting total number of pages of the Existing Document
                //int pageCount = reader1.NumberOfPages;

                // Create New Layer for Watermark
                PdfLayer layer = new PdfLayer("Layer", stamper.Writer);
                // Loop through each Page
                
                    // Getting the Page Size
                    Rectangle rect = reader1.GetPageSize(1);

                    // Get the ContentByte object
                    PdfContentByte cb = stamper.GetOverContent(1);

                    // Tell the cb that the next commands should be "bound" to this new layer
                    cb.BeginLayer(layer);

                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetColorFill(BaseColor.BLUE);
                    cb.SetFontAndSize(bf, 30);

                    cb.BeginText();
                    
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, UserName, rect.Width-700, rect.Height -360, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, CourseName, rect.Width-700, rect.Height -460, 0);
                    cb.SetFontAndSize(bf, 10);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Cdate, rect.Width - 800, rect.Height - 580, 0) ;
                    cb.EndText();

                    // Close the layer
                    cb.EndLayer();
                
            }
            
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