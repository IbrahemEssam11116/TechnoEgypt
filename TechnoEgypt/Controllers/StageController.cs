using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Models;

namespace TechnoEgypt.Controllers
{
    public class StageController : Controller
    {
        private readonly AppDBContext _dBContext;
        public StageController(AppDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public IActionResult Index()
        {
            var stages= _dBContext.Stages.ToList();
            return View(stages);
        }
        public async Task<IActionResult> AddOrEdit(int? Id)
        {
            ViewBag.PageName = Id == null ? "Create Stage" : "Edit Stage";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var stage = await _dBContext.Stages.FindAsync(Id);

                if (stage == null)
                {
                    return NotFound();
                }
                return View(stage);
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Stage stage)
        {
            bool IsEmployeeExist = false;

            var stageData = await _dBContext.Stages.FindAsync(stage.Id);

            if (stageData != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                stageData = new Stage();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stageData.Name = stage.Name;
                    stageData.AgeFrom = stage.AgeFrom;
                    stageData.AgeTo = stage.AgeTo;

                    if (IsEmployeeExist)
                    {
                        _dBContext.Update(stageData);
                    }
                    else
                    {
                        _dBContext.Stages.Add(stageData);
                    }
                    await _dBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index","Stage");
        }
    }
}
