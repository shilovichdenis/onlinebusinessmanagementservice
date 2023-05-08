using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Areas.Worker.Controllers
{
    [Area("Worker")]
    [Authorize(Roles = "Worker")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWorkerService _workerService;
        private readonly IImageService _imageService;
        private readonly IWorkerReviewService _workerReviewService;
        private readonly IServiceService _serviceService;
        private readonly IScheduleService _scheduleService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(IWorkerReviewService workerReviewService,
            IServiceService serviceService,
            IWorkerService workerService,
            UserManager<IdentityUser> userManager,
            IScheduleService scheduleService,
            IImageService imageService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _workerReviewService = workerReviewService;
            _serviceService = serviceService;
            _workerService = workerService;
            _userManager = userManager;
            _scheduleService = scheduleService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var worker = await _workerService.GetWorkerByUserId(user.Id);
                ViewData["Services"] = await _GetServices((int)worker.BusinessId) as List<Service>;
                return View(worker);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> _Manage()
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var worker = await _workerService.GetWorkerByUserId(user.Id);
                return View(worker);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Manage(WorkerViewModel model)
        {
            try
            {
                ModelState.Remove("User");
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Model is not valid");
                }

                await _workerService.UpdateWorker(model);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int workerId)
        {
            try
            {
                if (await _workerService.DeleteWorker(workerId))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var role = await _roleManager.FindByNameAsync("Worker");
                    if (role != null)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                        await _signInManager.SignInAsync(user, true);
                    } 
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    throw new ArgumentException("Worker does not deleted");
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
                await _workerReviewService.DeleteWorkerReview(reviewId);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        public async Task<List<Service>> _GetServices(int businessId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var worker = await _workerService.GetWorkerByUserId(user.Id);
            var wServices = await _serviceService.GetServicesByWorkerId(worker.WorkerId);
            var services = await _serviceService.GetServicesByBusinessId(businessId);
            var result = services.Except(wServices).ToList();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddService(int serviceId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var worker = await _workerService.GetWorkerByUserId(user.Id);
                await _workerService.AddService(worker.WorkerId, serviceId);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveService(int serviceId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var worker = await _workerService.GetWorkerByUserId(user.Id);
                await _workerService.RemoveService(worker.WorkerId, serviceId);
                return RedirectToAction("Index", "Account", new { area = "Worker" });
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
                    return RedirectToAction("Index", "Account", new { area = "Worker" });
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
                    return RedirectToAction("Index", "Account", new { area = "Worker" });
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

    }
}
