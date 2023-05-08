using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using OnlineBusinessManagementService.Models;
using System.Diagnostics;

namespace OnlineBusinessManagementService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertisementService _advertisementService;
        private readonly IServiceService _serviceService;
        private readonly IBusinessService _businessService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkerService _workerService;
        private readonly IFavoritesService _favoritesService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,
            IAdvertisementService advertisementService,
            IServiceService serviceService,
            IBusinessService businessService,
            IFavoritesService favoritesService,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICategoryService categoryService,
            IWorkerService workerService)
        {
            _logger = logger;
            _advertisementService = advertisementService;
            _serviceService = serviceService;
            _businessService = businessService;
            _categoryService = categoryService;
            _workerService = workerService;
            _favoritesService = favoritesService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        private async Task<HomeViewModel> GetHomeViewModel()
        {
            return new HomeViewModel()
            {
                Advertisements = await _advertisementService.GetAdvertisements(),
                Businesses = await _businessService.GetBusinesses(),
                Categories = await _context.Categories.ToListAsync()
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var favoriteBusinesses = await _favoritesService.GetFavoriteBusinesses(_userManager.GetUserId(User));
                var favorites = new List<BusinessViewModel>();
                foreach (var favorite in favoriteBusinesses)
                {
                    favorites.Add(await _businessService.GetBusiness(favorite.BusinessId));
                }
                ViewData["FavoriteBusinesses"] = favorites as List<BusinessViewModel>;
            }
            
            return View(await GetHomeViewModel());
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }

    }
}