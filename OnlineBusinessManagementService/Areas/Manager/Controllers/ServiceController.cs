using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace OnlineBusinessManagementService.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly ICategoryService _categoryService;

        public ServiceController(IServiceService serviceService, ICategoryService categoryService)
        {
            _serviceService = serviceService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> _Add(int businessId)
        {
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            ViewData["BusinessId"] = businessId;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServiceViewModel serviceModel)
        {
            try
            {
                ModelState.Remove("ImagePath");
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }
                await _serviceService.CreateService(serviceModel);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> _Edit(int? serviceId, int? businessId)
        {
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            ViewData["BusinessId"] = businessId;
            try
            {
                var service = await _serviceService.GetService(serviceId);
                return PartialView(await _serviceService.ToViewModel(service));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceViewModel serviceModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }

                await _serviceService.UpdateService(serviceModel);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Remove(int serviceId)
        {
            try
            {
                if (await _serviceService.DeleteService(serviceId))
                {
                    return RedirectToAction("Index", "Business", new { area = "Manager" });
                }
                else
                {
                    throw new InvalidOperationException("Service not deleted");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
