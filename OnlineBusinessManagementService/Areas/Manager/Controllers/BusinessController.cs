using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineBusinessManagementService.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class BusinessController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly IImageService _imageService;
        private readonly IBusinessReviewService _businessReviewService;
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BusinessController(IBusinessService businessService,
            IImageService imageService,
            IBusinessReviewService businessReviewService,
            IClientService clientService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _businessService = businessService;
            _businessReviewService = businessReviewService;
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return NotFound();
                }
                var business = await _businessService.GetBusinessByUserId(user.Id);
                ViewData["TotalPrice"] = business.Records.Sum(r => r.TotalPrice);
                ViewData["ClientsCount"] = _clientService.GetClientsByBusinessId(business.BusinessId).Result.Count();
                ViewData["RecordsCount"] = business.Records.Count();
                return View(business);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }

        }

        [HttpGet]
        public async Task<IActionResult> _Manage()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var business = await _businessService.GetBusinessByUserId(user.Id);
                ViewData["TotalPrice"] = business.Records.Sum(r => r.TotalPrice);
                ViewData["ClientsCount"] = _clientService.GetClientsByBusinessId(business.BusinessId).Result.Count();
                ViewData["RecordsCount"] = business.Records.Count();
                return PartialView(business);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Manage(BusinessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            try
            {
                await _businessService.UpdateBusiness(model);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int businessId)
        {
            try
            {
                if (await _businessService.DeleteBusiness(businessId))
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    await _userManager.RemoveFromRoleAsync(user, "Manager");
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    throw new InvalidOperationException("Business does not deleted");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult _AddImages(string directory)
        {
            ViewData["Directory"] = directory;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddImages(string directory, List<IFormFile> files)
        {
            try
            {
                if (_imageService.AddImagesTo(directory, files))
                {
                    return RedirectToAction("Index", "Business", new { area = "Manager" });
                }
                else
                {
                    throw new InvalidOperationException("Images does not added");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(string directory)
        {
            try
            {
                if (_imageService.DeleteImage(directory))
                {
                    return RedirectToAction("Index", "Business", new { area = "Manager" });
                }
                else
                {
                    throw new InvalidOperationException("Images does not added");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                await _businessReviewService.DeleteBusinessReview(reviewId);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
