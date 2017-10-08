using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Attributes;
using Stories.Constants;
using Stories.Models.Comment;
using Stories.Models.Flags;
using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService CommentService;
        private readonly IValidator<AddCommentViewModel> AddCommentViewModelValidator;
        private readonly IValidator<DeleteCommentModel> DeleteCommentModelValidator;
        private readonly IValidator<UpdateCommentModel> UpdateCommentModelValidator;
        private readonly IValidator<ToggleFlagModel> ToggleFlagModelValidator;
        private readonly IFlagService FlagService;

        public CommentController(ICommentService commentService,
                                 IFlagService flagService,
                                 IValidator<AddCommentViewModel> addCommentViewModelValidator, 
                                 IValidator<DeleteCommentModel> deleteCommentModelValidator,
                                 IValidator<UpdateCommentModel> updateCommentModelValidator,
                                 IValidator<ToggleFlagModel> toggleFlagModelValidator)
        {
            CommentService = commentService;
            AddCommentViewModelValidator = addCommentViewModelValidator;
            DeleteCommentModelValidator = deleteCommentModelValidator;
            UpdateCommentModelValidator = updateCommentModelValidator;
            ToggleFlagModelValidator = toggleFlagModelValidator;
            FlagService = flagService;
        }

        public async Task<IActionResult> Get(string hashId)
        {
            var model = await CommentService.Get(hashId);

            return PartialView($"~/Views/Shared/DisplayTemplates/{nameof(CommentViewModel)}.cshtml", model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Add(AddCommentViewModel model)
        {
            var validationResult = AddCommentViewModelValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            Guid userId;
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out userId))
            {
                return Json(new { Status = false, Messages = new string[] { "Error adding comment." } });
            }
            
            model.UserId = userId;

            var commentModel = await CommentService.Add(model);

            return Json(new { CommentHashId = commentModel.HashId, Status = true });
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Delete(DeleteCommentModel model)
        {            
            if(!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Messages = new string[] { "Error deleting comment." } });
            }

            model.UserId = userId;

            var validationResult = DeleteCommentModelValidator.Validate(model);

            if(!validationResult.IsValid)
            {
                return Json(new { Status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var isDeleted = await CommentService.Delete(model);
            return Json(new { Status = isDeleted });
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Update(UpdateCommentModel model)
        {
            if(!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Messages = new string[] { "Error updating comment." } });
            }

            model.UserId = userId;

            var validationResult = UpdateCommentModelValidator.Validate(model);

            if(!validationResult.IsValid)
            {
                return Json(new { status = validationResult.IsValid, Messages = validationResult.Messages });
            }

            var result = await CommentService.Update(model);

            return null;
        }

        [HttpPost]
        [Roles(Roles.User)]
        public async Task<IActionResult> Flag(string hashId)
        {
            var ids = new Hashids(minHashLength: 5);
            var commentId = ids.Decode(hashId).First();

            Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId);

            var flagModel = new ToggleFlagModel
            {
                CommentId = commentId,
                UserId = userId
            };

            var validationResult = ToggleFlagModelValidator.Validate(flagModel);

            if (!validationResult.IsValid)
            {
                return Json(new { status = false, messages = validationResult.Messages });
            }

            var status = await FlagService.ToggleFlag(flagModel);

            return Json(new { status = status });
        }
    }
}
