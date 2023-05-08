using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBusinessManagementService.Models;
using System.Data;

namespace OnlineBusinessManagementService.Areas.Worker.Controllers
{
    [Area("Worker")]
    [Authorize(Roles = "Worker")]
    public class RecordController : Controller
    {
        protected readonly IRecordService _recordService;
        protected readonly IServiceService _serviceService;
        protected readonly IClientService _clientService;
        protected readonly IWorkerService _workerService;
        protected readonly IScheduleService _scheduleService;
        protected readonly UserManager<IdentityUser> _userManager;
        public RecordController(IRecordService recordService,
            UserManager<IdentityUser> userManager,
            IScheduleService scheduleService,
            IServiceService serviceService,
            IClientService clientService,
            IWorkerService workerService)
        {
            _recordService = recordService;
            _userManager = userManager;
            _scheduleService = scheduleService;
            _serviceService = serviceService;
            _clientService = clientService;
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> EditRecord(int recordId)
        {
            try
            {
                var record = await _recordService.GetRecord(recordId);
                ViewData["Schedule"] = await _scheduleService.GetSchedules(record.WorkerId);
                ViewData["Services"] = await _recordService.GetServices(record.RecordId);
                return View(record);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    area = "",
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRecord(RecordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException();
                }

                await _recordService.UpdateRecord(model);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    area = "",
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int recordId)
        {
            try
            {
                await _recordService.DeleteRecord(recordId);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    area = "",
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> _AddService(int recordId, int workerId)
        {
            ViewData["RecordId"] = recordId;
            try
            {
                var services = await _GetServices(workerId, recordId);
                return PartialView(services);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddService(int recordId, int wServiceId)
        {
            try
            {
                if (await _recordService.AddServiceToRecord(recordId, wServiceId))
                {
                    return RedirectToAction("EditRecord", "Record", new { area = "Worker", recordId = recordId });
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveService(int recordId, int wServiceId)
        {
            try
            {
                if (await _recordService.RemoveServiceFromRecord(recordId, wServiceId))
                {
                    return RedirectToAction("EditRecord", "Record", new { area = "", recordId = recordId });
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        public async Task<List<WorkerServices>> _GetServices(int workerId, int recordId)
        {
            var wServices = await _serviceService.GetWorkerServicesByWorkerId(workerId);
            var newServices = new List<WorkerServices>();
            var rServices = await _recordService.GetServices(recordId);
            foreach (var wService in wServices)
            {
                if (!rServices.Any(r => r.ServiceId == wService.Id))
                {
                    newServices.Add(wService);
                }
            }
            return newServices;
        }
    }
}
