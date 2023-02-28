using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TechnoEgypt.Models;
using TechnoEgypt.ViewModel;

namespace TechnoEgypt.Controllers
{
    public class CourseCategoryController : Controller

    {
        private readonly AppDBContext _dBContext;
        public CourseCategoryController(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public IActionResult Index()
        {

            var coursecategory = _dBContext.CourseCategories
                .Include(CourseCategories => CourseCategories.Stage).Select(w => new CourseCategoryIndex { Id = w.Id, Name = w.Name, TrackName = w.Stage.Name }).ToList();
            
            return View(coursecategory);
        }
        public async Task<IActionResult> AddOrEdit(int? Id)
        {
           
            ViewBag.PageName = Id == null ? "Create Track" : "Edit Track";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var coursecategory = await _dBContext.CourseCategories.Where(w=>w.Id==Id)
                    .Include(CourseCategories => CourseCategories.Stage).
                    Select(w => new CourseCategoryIndex { Id = w.Id, Name = w.Name, TrackName = w.Stage.Name })
                    .FirstOrDefaultAsync();
                coursecategory.Stages = _dBContext.Stages.ToList();

                if (coursecategory == null)
                {
                    return NotFound();
                }
                return View(coursecategory);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(CourseCategory coursecategory)
        {

            // coursecategory.Stages = _dBContext.Stages.ToList();
            

            bool IsEmployeeExist = false;

            var coursecategoryData = await _dBContext.CourseCategories.FindAsync(coursecategory.Id);
                
            
            if (coursecategoryData != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                coursecategoryData = new CourseCategory();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    coursecategoryData.Name = coursecategoryData.Name;
                    coursecategoryData.StageId = coursecategoryData.StageId;
                   // stageData.AgeTo = stage.AgeTo;

                    if (IsEmployeeExist)
                    {
                        _dBContext.Update(coursecategoryData);
                    }
                    else
                    {
                        _dBContext.CourseCategories.Add(coursecategoryData);
                    }
                    await _dBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "CourseCategory");
        }
    }
}
