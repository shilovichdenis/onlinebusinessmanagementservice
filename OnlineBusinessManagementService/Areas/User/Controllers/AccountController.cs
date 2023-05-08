using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBusinessManagementService.Areas.UserArea.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFavoritesService _favoritesService;
        private readonly IServiceService _serviceService;
        private readonly IBusinessService _businessService;
        private readonly IWorkerReviewService _workerReviewService;
        private readonly IBusinessReviewService _businessReviewService;

        public AccountController(
            UserManager<IdentityUser> userManager,
            IFavoritesService favoritesService,
            IServiceService serviceService,
            IBusinessService businessService,
            IWorkerReviewService workerReviewService,
            IBusinessReviewService businessReviewService)
        {
            _userManager = userManager;
            _favoritesService = favoritesService;
            _serviceService = serviceService;
            _businessService = businessService;
            _workerReviewService = workerReviewService;
            _businessReviewService = businessReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoriteService(int serviceId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var service = await _serviceService.GetServiceViewModel(serviceId);
                await _favoritesService.AddFavorite(user.Id, serviceId, service.GetType());
                return RedirectToAction("Details", "Service", new { area = "", serviceId = serviceId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteService(int serviceId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var service = await _serviceService.GetService(serviceId);
                await _favoritesService.RemoveFavorite(user.Id, serviceId, service.GetType());
                return RedirectToAction("Details", "Service", new { area = "", serviceId = serviceId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoriteBusiness(int businessId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var business = await _businessService.GetBusiness(businessId);
                await _favoritesService.AddFavorite(user.Id, businessId, business.GetType());
                return RedirectToAction("Details", "Business", new { area = "", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteBusiness(int businessId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var business = await _businessService.GetBusiness(businessId);
                await _favoritesService.RemoveFavorite(user.Id, businessId, business.GetType());
                return RedirectToAction("Details", "Business", new { area = "", businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
