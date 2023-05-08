using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBusinessManagementService.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class WorkerController : Controller
    {
        private readonly IWorkerService _workerService;
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;

        public WorkerController(
            IWorkerService workerService,
            IBusinessService businessService,
            IUserService userService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _workerService = workerService;
            _businessService = businessService;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int businessId)
        {
            var workers = await _workerService.GetWorkersByBusinessId(businessId);
            return View(workers);
        }

        [HttpGet]
        public async Task<IActionResult> _Add(int businessId)
        {
            ViewData["BusinessId"] = businessId;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string? email, int? businessId)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                await _businessService.AddWorker(user, businessId);
                var role = await _roleManager.FindByNameAsync("Worker");
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int? workerId)
        {
            try
            {
                await _businessService.RemoveWorker(workerId);
                return RedirectToAction("Index", "Business", new { area = "Manager" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
