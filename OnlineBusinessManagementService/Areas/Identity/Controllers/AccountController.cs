using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OnlineBusinessManagementService.Areas.Identity.Models;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace OnlineBusinessManagementService.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IEmailSender _emailSender;
        private readonly IFavoritesService _favoritesService;
        private readonly IServiceService _serviceService;
        private readonly IBusinessService _businessService;
        private readonly IWorkerReviewService _workerReviewService;
        private readonly IRecordService _recordService;
        private readonly IBusinessReviewService _businessReviewService;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            IEmailSender emailSender,
            IFavoritesService favoritesService,
            IServiceService serviceService,
            IRecordService recordService,
            IBusinessService businessService,
            IWorkerReviewService workerReviewService,
            IBusinessReviewService businessReviewService,
            IUserService userService,
            IImageService imageService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _favoritesService = favoritesService;
            _serviceService = serviceService;
            _recordService = recordService;
            _businessService = businessService;
            _workerReviewService = workerReviewService;
            _businessReviewService = businessReviewService;
            _userService = userService;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var imagePath = _imageService.AddImage(Path.Combine("users", model.Email.ToLower()), model.Photo);

                var user = new User()
                {
                    Name = model.Name,
                    SurName = model.SurName,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhotoPath = imagePath != null ? imagePath : @"images\default\defaultuser.jpg"
                };

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                    pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var model = await _userService.GetUserViewModelById(user.Id);
                ViewData["Records"] = await _recordService.GetRecordsByUserId(user.Id) as List<RecordViewModel>;
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Manage(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                return View(userModel);
            }

            try
            {
                var user = await _userService.GetUserById(userModel.UserId);
                UserViewModel.UpdateData(userModel, ref user);

                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Manage", "Account", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                return PartialView(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    throw new InvalidOperationException();
                }

                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Manage", "Account", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { area = "", message = ex.Message });
            }
        }
    }
}
