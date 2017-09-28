using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Models.Users;
using Stories.Models.ViewModels.Administration;
using Stories.Models.ViewModels.User;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    [Roles(Roles.Moderator, Roles.Admin)]
    public class AdministrationController : BaseController
    {
        private readonly IBanService BanService;
        private readonly IUserService UserService;
        private readonly IValidator<CreateUserViewModel> CreateUserValidator;
        private readonly IValidator<BanUserModel> BanUserModelValidator;

        public AdministrationController(IUserService userService, IValidator<CreateUserViewModel> createUserValidator, IBanService banService, IValidator<BanUserModel> banUserModelValidator)
        {
            BanService = banService;
            UserService = userService;
            CreateUserValidator = createUserValidator;
            BanUserModelValidator = banUserModelValidator;
        }

        public async Task<IActionResult> Index()
        {
            var model = await UserService.GetUsers(0, 15);
            return View(model);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var currentUser = CurrentUser.IsInRole(Roles.Admin);

            if (!Guid.TryParse(id, out Guid userId))
            {
                return StatusCode(404);
            }

            var user = await UserService.GetUser(userId);

            var model = new UserViewModel
            {
                BanReason = user.BanModel.Reason,
                CreatedDate = user.CreatedDate.ToString("o"),
                IsBanned = user.IsBanned,
                Username = user.Username,
            };

            return View(model);
        }

        [Roles(Roles.Admin)]
        public IActionResult CreateUser()
        {
            var model = new CreateUserViewModel();

            if (CurrentUser.IsInRole(Roles.Admin))
            {
                model.Roles.Add(Roles.Admin);
                model.Roles.Add(Roles.Moderator);
            }

            return View(model);
        }

        [Roles(Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);

                return Json(new { Status = false, Message = string.Join("\n", errors) });
            }

            var validationResult = CreateUserValidator.Validate(model);

            if(!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var userModel = await UserService.CreateUser(new CreateUserModel {
                ConfirmPassword = model.ConfirmPassword,
                Email = model.Email,
                Password = model.Password,
                Roles = model.Roles,
                Username = model.Username
            });

            if (userModel == null)
                return Json(new { Status = false, Messages = new string[] { "Error creating user." } });

            return Json(new { Status = true });
        }

        [Roles(Roles.Admin, Roles.Moderator)]
        public async Task<IActionResult> BanUser(Guid userId)
        {
            var user = await UserService.GetUser(userId);

            if (user == null)
                return NotFound();

            var model = new BanUserViewModel
            {
                UserId = userId,
                ExpiryDate = user.BanModel.ExpiryDate,
                Notes = user.BanModel.Notes,
                Reason = user.BanModel.Reason,
                Username = user.Username
            };

            return View(model);
        }

        [Roles(Roles.Admin, Roles.Moderator)]
        [HttpPost]
        public async Task<IActionResult> BanUser(BanUserViewModel model)
        {
            if(!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Messages = new string[] { "User not found." } });
            }

            var banModel = new BanUserModel
            {
                BannedByUserId = userId,
                ExpiryDate = model.ExpiryDate,
                Notes = model.Notes,
                Reason = model.Reason,
                UserId = model.UserId
            };

            var validationResult = BanUserModelValidator.Validate(banModel);

            if (!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            if(await BanService.BanUser(banModel))
            {
                return Json(new { Status = true });
            }

            return Json(new { Status = false, Messages = new string[] { "Unknown error occured" } });
        }
    }
}
