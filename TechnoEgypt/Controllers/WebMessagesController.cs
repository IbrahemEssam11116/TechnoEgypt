using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
using TechnoEgypt.Services;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
    public class WebMessagesController : Controller
    {
        private readonly UserDbContext _dBContext;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<AppUser> _userManager;
        private readonly NotificationSevice _notificationSevice;

        public WebMessagesController(UserDbContext dBContext, UserManager<AppUser> userManager, IWebHostEnvironment env, NotificationSevice notificationSevice)
        {
            _dBContext = dBContext;
            this.env = env;
            _userManager = userManager;
            _notificationSevice = notificationSevice;
        }
        public IActionResult Index()
        {

            var Messages = _dBContext.ChildMessages
                .Include(Messages => Messages.Parent).Include(w => w.CreatedUser)
                .Select(w => new WebMessagesIndex
                {
                    Date = w.Date == null ? "" : w.Date.Value.ToString("MM/dd/yyyy")
                ,
                    Id = w.Id,
                    Title = w.Title,
                    Message = w.Message,
                    SenderName = w.CreatedUser != null ? w.CreatedUser.UserName : ""
                })
                .ToList();

            return View(Messages);


        }
        [HttpGet]
        public IActionResult CreateMessage()
        {
            ViewBag.Messages = new SelectList(_dBContext.ChildMessages.ToList(), "Id", "Name");
            return PartialView(new WebMessagesIndex());
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(WebMessagesIndex model)
        {
            var ParentList = _dBContext.Parents.ToList();
            var userId = this.User.Identity.GetUserId();

            foreach (var item in ParentList)
            {
                ParentMessage MessageInfo = new ParentMessage();
                MessageInfo.ParentId = item.Id;
                MessageInfo.Message = model.Message;
                MessageInfo.Title = model.Title;
                MessageInfo.CreatedUserId = userId;
                MessageInfo.Date = DateTime.Now;
                _dBContext.ChildMessages.Add(MessageInfo);
                await _notificationSevice.SendNotification(model.Title, model.Message, item.Id);
            }
            _dBContext.SaveChanges();
            return RedirectToAction("Index", "WebMessages");
        }
        //public async Task<IActionResult> AddOrEdit(int? Id)
        //{
        //    WebCourseIndex courses = new WebCourseIndex();
        //    ViewBag.PageName = Id == null ? "Create Course" : "Edit Course";
        //    ViewBag.IsEdit = Id == null ? false : true;
        //    if (Id == null)
        //    {

        //        courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
        //        courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);
        //        courses.ToolList = new SelectList(_dBContext.CourseToolsMyProperty.ToList(), "Id", "Name", courses.ToolID);
        //        return View(courses);
        //    }
        //    else
        //    {

        //        courses = await _dBContext.Courses.Where(w => w.Id == Id)
        //           .Include(Courses => Courses.courseTool).Include(Courses => Courses.CourseCategory).ThenInclude(CourseCategory => CourseCategory.Stage)
        //        .Select(w => new WebCourseIndex { Id = w.Id, Name = w.Name, StageId = w.CourseCategory.Stage.Id, StageName = w.CourseCategory.Stage.Name, CourseCategoryId = w.CourseCategoryId, CourseCategoryName = w.CourseCategory.Name, ToolID = w.ToolId, ToolName = w.courseTool.Name })
        //        .FirstOrDefaultAsync();
        //        courses.course = await _dBContext.Courses.Where(w => w.Id == Id).FirstOrDefaultAsync();
        //        courses.Stages = _dBContext.Stages.ToList();
        //        courses.CourseCategories = _dBContext.CourseCategories.ToList();
        //        courses.Tools = _dBContext.CourseToolsMyProperty.ToList();
        //        courses.StageList = new SelectList(_dBContext.Stages.ToList(), "Id", "Name", courses.StageId);
        //        courses.CourseCategoryList = new SelectList(_dBContext.CourseCategories.ToList(), "Id", "Name", courses.CourseCategoryId);
        //        courses.ToolList = new SelectList(_dBContext.CourseToolsMyProperty.ToList(), "Id", "Name", courses.ToolID);
        //        if (courses == null)
        //        {
        //            return NotFound();
        //        }
        //        return View(courses);
        //    }

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrEdit(WebCourseIndex webcourse)
        //{

        //    // coursecategory.Stages = _dBContext.Stages.ToList();
        //    string IImageName = "";
        //    if (webcourse.Image != null)
        //    {
        //        var FilePath = "Files\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Millisecond.ToString() + webcourse.Image.FileName;
        //        var path = env.WebRootPath + "\\" + FilePath;
        //        using (FileStream fs = System.IO.File.Create(path))
        //        {
        //            webcourse.Image.CopyTo(fs);
        //        }
        //        IImageName = FilePath;
        //    }
        //    bool IsEmployeeExist = false;

        //    var courseData = await _dBContext.Courses.FindAsync(webcourse.Id);


        //    if (courseData != null)
        //    {
        //        IsEmployeeExist = true;
        //        if (courseData.ImageURL != null && IImageName == "")
        //        {
        //            IImageName = courseData.ImageURL;
        //        }
        //    }
        //    else
        //    {
        //        courseData = new Course();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //courseData = webcourse.course;
        //            courseData.Id = webcourse.Id;
        //            courseData.Name = webcourse.Name;
        //            courseData.ToolId = webcourse.ToolID;
        //            courseData.CourseCategoryId = webcourse.CourseCategoryId;
        //            courseData.ImageURL = IImageName;
        //            courseData.ArDescripttion = webcourse.course.ArDescripttion;
        //            courseData.ArName = webcourse.course.ArName;
        //            courseData.CognitiveAbilities = webcourse.course.CognitiveAbilities;
        //            courseData.Concentration = webcourse.course.Concentration;
        //            courseData.CriticalThinking = webcourse.course.CriticalThinking;
        //            courseData.LogicalThinking = webcourse.course.LogicalThinking;
        //            courseData.DataCollectionandAnalysis = webcourse.course.DataCollectionandAnalysis;
        //            courseData.MathematicalReasoning = webcourse.course.MathematicalReasoning;
        //            courseData.ProblemSolving = webcourse.course.ProblemSolving;
        //            courseData.Planning = webcourse.course.Planning;
        //            courseData.Innovation = webcourse.course.Innovation;
        //            courseData.SocialLifeSkills = webcourse.course.SocialLifeSkills;
        //            courseData.ScientificResearch = webcourse.course.ScientificResearch;


        //            if (IsEmployeeExist)
        //            {
        //                _dBContext.Entry<Course>(courseData).State = EntityState.Modified;
        //            }
        //            else
        //            {
        //                _dBContext.Courses.Add(courseData);
        //            }
        //            await _dBContext.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return RedirectToAction("Index", "WebCourse");
        //}
        //public async Task<ActionResult> Delete(int Id)
        //{
        //    var course = await _dBContext.Courses.FindAsync(Id);
        //    _dBContext.Entry(course).State = EntityState.Deleted;
        //    await _dBContext.SaveChangesAsync();
        //    //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

        //    return RedirectToAction("Index");
        //}




    }
}