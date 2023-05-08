using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBusinessManagementService.Services;

namespace OnlineBusinessManagementService.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkerReviewService _workerReviewService;
        private readonly UserManager<IdentityUser> _userManager; protected readonly IWorkerService _workerService;
        public WorkerController(IWorkerService workerService, 
            IWorkerReviewService workerReviewService, 
            UserManager<IdentityUser> userManager)
        {
            _workerReviewService = workerReviewService;
            _userManager = userManager;
            _workerService = workerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int serviceId)
        {
            try
            {
                var workers = await _workerService.GetWorkersByServiceId(serviceId);
                return View(workers);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int workerId)
        {
            try
            {
                var worker = await _workerService.GetWorker(workerId);
                return View(worker);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(string description, int workerId, int rating)
        {
            try
            {
                await _workerReviewService.CreateWorkerReview(new WorkerReviewViewModel() { WorkerId = workerId, Rating = rating, Description = description, UserId = _userManager.GetUserId(User) });
                return RedirectToAction("Details", "Worker", new { workerId = workerId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
