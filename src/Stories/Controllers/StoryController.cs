using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Models.Story;
using Stories.Models.StoryViewModels;
using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class StoryController : BaseController
    {
        private IStoryService StoryService { get; set; }
        private IUserService UserService { get; set; }
        private IValidator<CreateStoryModel> CreateStoryModelValidator { get; set; }

        public StoryController(IStoryService storyService, IUserService userService, IValidator<CreateStoryModel> createStoryModelValidator)
        {
            StoryService = storyService;
            UserService = userService;
            CreateStoryModelValidator = createStoryModelValidator;
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
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Message = "Error adding story." });
            }

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
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Message = "Error deleting the story." });
            }

            var model = await StoryService.Get(hashId, userId);

            if (model == null)
            {
                return Json(new { Status = false, Message = "Error deleting the story." });
            }

            if (!(CurrentUser.IsInRole(Roles.Admin) || CurrentUser.IsInRole(Roles.Moderator)))
            {
                if (!model.Summary.IsSubmitter)
                {
                    return Json(new { Status = false, Message = "You cannot deleted stories submitted by others." });
                }

                var submitDate = DateTime.Parse(model.Summary.SubmittedDate);

                if (!model.Summary.IsDeletable)
                {
                    return Json(new { Status = false, Message = "Threshold for story deletion has expired. Moderator intervention is required to delete this story." });
                }
            }

            var status = await StoryService.Delete(model.Summary.HashId);

            return Json(new { Status = status });
        }
    }
}
