using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stories.Constants;
using Stories.Models.Users;
using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class AuthController : BaseController
    {
        private readonly Services.IAuthenticationService AuthenticationService;
        private readonly IUserService UserService;
        private readonly IValidator<LoginViewModel> LoginViewModelValidator;
        private readonly ILogger<AuthController> logger;
        private readonly IValidator<SignupViewModel> SignupValidator;

        public AuthController(Services.IAuthenticationService authenticationService, IUserService userService, IValidator<SignupViewModel> signupValidator, 
                              IValidator<LoginViewModel> loginViewModelValidator, ILogger<AuthController> logger)
        {
            AuthenticationService = authenticationService;
            UserService = userService;
            SignupValidator = signupValidator;
            LoginViewModelValidator = loginViewModelValidator;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);

                return Json(new { Status = false, Messages = errors });
            }

            var validationResult = SignupValidator.Validate(model);

            if(!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var userModel = await UserService.CreateUser(new CreateUserModel
            {
                ConfirmPassword = model.ConfirmPassword,
                Email = model.Email,
                Password = model.Password,
                Roles = new List<string> { Roles.User },
                Username = model.Username
            });

            return Json(new { Status = await SignInUser(userModel) });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (CurrentUser.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (CurrentUser.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var validationResult = LoginViewModelValidator.Validate(model);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);

                return Json(new { Status = false, Messages = errors });
            }

            if (!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var user = await AuthenticationService.AuthenticateUser(model.Email, model.Password);

            var status = await SignInUser(user);

            if (status == false)
            {
                return Json(new { Status = status, Messages = new string[] { "Invalid email or password" } });
            }

            return Json(new { Status = status });
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task<bool> SignInUser(UserModel user)
        {
            if (user == null)
            {
                return false;
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            },
            authenticationType: "ApplicationCookie");

            foreach (var role in user.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return true;
        }
    }
}
