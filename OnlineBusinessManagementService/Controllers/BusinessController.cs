using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBusinessManagementService.Models;

namespace OnlineBusinessManagementService.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBusinessReviewService _businessReviewService;
        private readonly IUserService _userService;
        private readonly IWorkerService _workerService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public BusinessController(IBusinessService businessService,
            UserManager<IdentityUser> userManager,
            IBusinessReviewService businessReviewService,
            IUserService userService,
            IWorkerService workerService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _businessService = businessService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _workerService = workerService;
            _userService = userService;
            _businessReviewService = businessReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int categoryId)
        {
            return View(await _businessService.GetBusinessesByCategoryId(categoryId));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BusinessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var business = await _businessService.CreateBusiness(model);
                if (business != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (model.Category == CategoryOfBusiness.IndividualEntrepreneur)
                    {
                        var userView = await _userService.GetUserById(user.Id);
                        var roleW = await _roleManager.FindByNameAsync("Worker");
                        if (roleW != null)
                        {
                            await _userManager.AddToRoleAsync(user, roleW.Name);
                            await _businessService.AddWorker(userView, business.Id);
                        }
                    }
                    var role = await _roleManager.FindByNameAsync("Manager");
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                        await _signInManager.SignInAsync(user, true);
                    }
                    return RedirectToAction("Index", "Business", new { area = "Manager" });
                }
                else
                {
                    throw new ArgumentNullException();
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int businessId)
        {
            try
            {
                return View(await _businessService.GetBusiness(businessId));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(string description, int businessId, int rating)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var review = new BusinessReviewViewModel() { BusinessId = businessId, Rating = rating, Description = description, UserId = user.Id };
                await _businessReviewService.CreateBusinessReview(review);
                return RedirectToAction("Details", "Business", new { businessId = businessId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
