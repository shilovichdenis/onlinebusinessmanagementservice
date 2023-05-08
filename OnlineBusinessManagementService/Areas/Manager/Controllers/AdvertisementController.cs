using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace OnlineBusinessManagementService.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class AdvertisementController : Controller
    {
        protected readonly IAdvertisementService _advertisementService;
        protected readonly IServiceService _serviceService;

        public AdvertisementController(IAdvertisementService advertisementService, IServiceService serviceService)
        {
            _advertisementService = advertisementService;
            _serviceService = serviceService;
        }

        [HttpGet]
        public IActionResult _Create(int businessId)
        {
            ViewData["BusinessId"] = businessId;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }

                await _advertisementService.CreateAdvertisement(model);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> _AddService(int advertisementId, int businessId)
        {
            ViewData["AdvertisementId"] = advertisementId;
            try
            {
                var services = await _GetServices(businessId, advertisementId);
                return PartialView(services);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddService(int advertisementId, int serviceId)
        {
            try
            {
                await _advertisementService.AddService(advertisementId, serviceId);
                return RedirectToAction("Index", "Business", new { area = "Manager" });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveService(int advertisementId, int serviceId)
        {
            try
            {
                await _advertisementService.RemoveService(advertisementId, serviceId);
                return RedirectToAction("Index", "Business", new { area = "Manager" });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> _Edit(int advertisementId, int businessId)
        {
            ViewData["BusinessId"] = businessId;
            try
            {
                var advertisement = await _advertisementService.GetAdvertisement(advertisementId);
                return PartialView(advertisement);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdvertisementViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }

                await _advertisementService.UpdateAdvertisement(model);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? advertisementId)
        {
            try
            {
                await _advertisementService.DeleteAdvertisement(advertisementId);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        public async Task<List<Service>> _GetServices(int businessId, int advertisementId)
        {
            var advertisementServices = await _serviceService.GetServicesByAdvertisementId(advertisementId);
            var services = await _serviceService.GetServicesByBusinessId(businessId);
            return services.Except(advertisementServices).ToList();
        }


    }
}
