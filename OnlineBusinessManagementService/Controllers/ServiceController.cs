using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBusinessManagementService.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _serviceService.GetServices());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int serviceId)
        {
            try
            {
                return View(await _serviceService.GetServiceViewModel(serviceId));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
