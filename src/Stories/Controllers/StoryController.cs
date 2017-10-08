using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Models.Flags;
using Stories.Models.Stories;
using Stories.Models.StoryViewModels;
using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class StoryController : BaseController
    {
        private readonly IStoryService StoryService;
        private readonly IUserService UserService;
        private readonly IValidator<CreateStoryModel> CreateStoryModelValidator;
        private readonly IValidator<DeleteStoryModel> DeleteStoryModelValidator;
        private readonly IFlagService FlagService;
        private readonly IValidator<ToggleFlagModel> ToggleFlagModelValidator;

        public StoryController(IStoryService storyService, 
                               IUserService userService, 
                               IFlagService flagService,
                               IValidator<CreateStoryModel> createStoryModelValidator, 
                               IValidator<DeleteStoryModel> deleteStoryModelValidator,
                               IValidator<ToggleFlagModel> toggleFlagModelValidator)
        {
            StoryService = storyService;
            UserService = userService;
            CreateStoryModelValidator = createStoryModelValidator;
            DeleteStoryModelValidator = deleteStoryModelValidator;
            FlagService = flagService;
            ToggleFlagModelValidator = toggleFlagModelValidator;
        }

        public async Task<IActionResult> Index(string hashId)
        {
            StoryViewModel model = null;

            if (Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                model = await StoryService.Get(hashId, userId);
                if (CurrentUser.IsInRole(Roles.Moderator))
                {
                    model.Summary.IsDeletable = true;
                }
            }
            else
            {
                model = await StoryService.Get(hashId, null);
            }

            if (model == null)
                return NotFound();

            return View(model);
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);

            var createStoryModel = new CreateStoryModel
            {
                DescriptionMarkdown = model.DescriptionMarkdown,
                Id = model.Id,
                IsAuthor = model.IsAuthor,
                Title = model.Title,
                Url = model.Url,
                UserId = userId,
            };

            var validationResult = CreateStoryModelValidator.Validate(createStoryModel);

            if (!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var summary = await StoryService.Create(createStoryModel);

            return Json(new { Status = true });
        }

        [HttpPost]
        [Roles(Roles.Admin, Roles.Moderator, Roles.User)]
        public async Task<IActionResult> Delete(string hashId)
        {
            var ids = new Hashids(minHashLength: 5);
            var storyId = ids.Decode(hashId).First();

            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);
            var deleteModel = new DeleteStoryModel { StoryId = storyId, UserId = userId };

            var validationResult = DeleteStoryModelValidator.Validate(deleteModel);

            if(!validationResult.IsValid)
            {
                return Json(new { status = false, messages = validationResult.Messages });
            }

            var status = await StoryService.Delete(deleteModel);

            return Json(new { Status = status });
        }

        [HttpPost]
        [Roles(Roles.User)]
        public async Task<IActionResult> Flag(string hashId)
        {
            var ids = new Hashids(minHashLength: 5);
            var storyId = ids.Decode(hashId).First();

            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);

            var flagModel = new ToggleFlagModel
            {
                StoryId = storyId,
                UserId = userId
            };

            var validationResult = ToggleFlagModelValidator.Validate(flagModel);

            if(!validationResult.IsValid)
            {
                return Json(new { status = false, messages = validationResult.Messages });
            }

            var status = await FlagService.ToggleFlag(flagModel);

            return Json(new { status = status });
        }
    }
}
