using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBusinessManagementService.Controllers
{
    public class AdvertisementController : Controller
    {
        protected readonly IAdvertisementService _advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? advertisementId)
        {
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
    }
}
