using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoEgypt.Areas.Identity.Data;
using TechnoEgypt.Models;
namespace TechnoEgypt.Controllers
{
    [Authorize]
    public class StageController : Controller
    {
        private readonly UserDbContext _dBContext;

        public StageController(UserDbContext dBContext)
        {
            _dBContext = dBContext;
        }
        public IActionResult Index()
        {

            var stages = _dBContext.Stages.ToList();
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
                    stageData.ArName = stage.ArName;

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
            return RedirectToAction("Index", "Stage");
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
        public async Task<ActionResult> Delete(int Id)
        {
            var stage = await _dBContext.Stages.FindAsync(Id);
            _dBContext.Entry(stage).State = EntityState.Deleted;
            await _dBContext.SaveChangesAsync();
            //AddSweetNotification("Done", "Done, Deleted successfully", NotificationHelper.NotificationType.success);

            return RedirectToAction("Index");
        }
    }
}
