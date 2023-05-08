using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace OnlineBusinessManagementService.Areas.Worker.Controllers
{
    [Area("Worker")]
    [Authorize(Roles = "Worker")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> _Add(int workerId)
        {
            ViewData["WorkerId"] = workerId;
            ViewData["StartTime"] = new SelectList(await _scheduleService.GetTimeForSchedule(), "TimeInt", "TimeString");
            ViewData["EndTime"] = new SelectList(await _scheduleService.GetTimeForSchedule(), "TimeInt", "TimeString");
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ScheduleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }

                await _scheduleService.AddSchedule(model);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int scheduleId)
        {
            try
            {
                await _scheduleService.RemoveSchedule(scheduleId);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
