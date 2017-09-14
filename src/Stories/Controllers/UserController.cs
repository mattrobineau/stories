using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Models.Users;
using Stories.Models.ViewModels;
using Stories.Models.ViewModels.User;
using Stories.Services;
using Stories.Validation.BusinessRules;
using Stories.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService UserService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IEmailRule EmailBusinessRule;
        private readonly IMailService MailService;
        private readonly IReferralService ReferralService;
        private readonly IValidator<SignupViewModel> SignupValidator;
        private readonly IValidator<ChangePasswordModel> ChangePasswordValidator;

        public UserController(IUserService userService, IAuthenticationService authenticationService, IEmailRule emailBusinessRule, IMailService mailService, IReferralService referralService,
                              IValidator<SignupViewModel> signupValidator, IValidator<ChangePasswordModel> changePasswordValidator)
        {
            UserService = userService;
            AuthenticationService = authenticationService;
            EmailBusinessRule = emailBusinessRule;
            MailService = mailService;
            ReferralService = referralService;
            SignupValidator = signupValidator;
            ChangePasswordValidator = changePasswordValidator;
        }

        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Index()
        {

            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);

            var userModel = await UserService.GetUser(userId);

            var model = new UserViewModel
            {
                ChangePasswordViewModel = new ChangePasswordViewModel(),
                Username = userModel.Username,
                IsBanned = userModel.IsBanned,
                BanReason = userModel.BanModel.Reason,
                CreatedDate = userModel.CreatedDate.ToString("o")
            };

            return View(model);
        }

        [Route("user/{username}")]
        public async Task<IActionResult> ViewUser(string username)
        {
            var userModel = await UserService.GetUser(username);

            var model = new UserViewModel
            {
                Username = userModel.Username,
                IsBanned = userModel.IsBanned,
                BanReason = userModel.BanModel.Reason,
                CreatedDate = userModel.CreatedDate.ToString("o"),
                //RecentComments = ,
                //RecentStories = 
            };
            return View();
        }

        [Authorize(Roles = Roles.User)]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword.cshtml", new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            Guid userId;
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out userId))
            {
                return Json(new { Status = false, Message = "Could not find user" });
            }

            var changePasswordModel = new ChangePasswordModel
            {
                ConfirmNewPassword = model.ConfirmPassword,
                NewPassword = model.NewPassword,
                OldPassword = model.OldPassword,
                UserId = userId
            };

            var result = await AuthenticationService.ChangePassword(changePasswordModel);

            return Json(new { Status = result });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/referral/{code}")]
        public async Task<IActionResult> Referral(string code)
        {
            var model = new ReferralViewModel { Code = code, CodeIsValid = true };

            if (!Guid.TryParse(code, out Guid referralCode))
            {
                model.CodeIsValid = false;
                return View(model);
            }

            var referral = await ReferralService.Get(referralCode);

            if (referral == null)
            {
                model.CodeIsValid = false;
                return View(model);
            }

            model.Email = referral.Email;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Referral(ReferralViewModel model)
        {
            var validationResult = SignupValidator.Validate((SignupViewModel)model);

            if (!validationResult.IsValid)
                return Json(new { Status = false, Messages = validationResult.Messages });

            if (!Guid.TryParse(model.Code, out Guid code))
                return Json(new { Status = false, Messages = new List<string> { "Invalid referral code." } });

            var referral = ReferralService.Get(code);

            if (referral == null)
                return Json(new { Status = false, Messages = new List<string> { "Invalid referral code." } });

            var userModel = await UserService.CreateUser(new CreateUserModel
            {
                ConfirmPassword = model.ConfirmPassword,
                Email = model.Email,
                Password = model.Password,
                Roles = new List<string> { Roles.User },
                Username = model.Username
            });

            return Json(new { Status = true });
        }

        [Roles(Roles.User)]
        public IActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [Roles(Roles.User)]
        public async Task<IActionResult> Invite(string email)
        {
            if (!EmailBusinessRule.Validate(email))
                return Json(new { Status = false, Message = "Invalid email address." });

            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);

            if (!await ReferralService.SendInvite(email, userId))
                return Json(new { Status = false, Message = "Error sending email. Try again later." });

            return Json(new { Status = true });
        }
    }
}
