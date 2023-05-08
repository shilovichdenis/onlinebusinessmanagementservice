using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Areas.UserArea.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class RecordController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IRecordService _recordService;

        public RecordController(IUserService userService, UserManager<User> userManager, IRecordService recordService)
        {
            _userService = userService;
            _userManager = userManager;
            _recordService = recordService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var records = _recordService.GetRecordsByUserId(_userManager.GetUserId(User));
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? recordId)
        {
            try
            {
                var record = await _recordService.GetRecord(recordId);
                return PartialView(record);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecordViewModel model)
        {
            try
            {
                var record = await _recordService.CreateRecord(model);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? recordId)
        {
            try
            {
                var record = await _recordService.GetRecord(recordId);
                return View(record);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecordViewModel model)
        {
            try
            {
                var record = await _recordService.UpdateRecord(model);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult DeleteConfirm()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? recordId)
        {
            try
            {
                var record = await _recordService.DeleteRecord(recordId);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
