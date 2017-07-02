using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService CommentService;
        private readonly IValidator<AddCommentViewModel> AddCommentViewModelValidator;
        public CommentController(ICommentService commentService, IValidator<AddCommentViewModel> addCommentViewModelValidator )
        {
            CommentService = commentService;
            AddCommentViewModelValidator = addCommentViewModelValidator;
        }

        public async Task<IActionResult> Get(string hashId)
        {
            var model = await CommentService.Get(hashId);

            return PartialView($"~/Views/Shared/DisplayTemplates/{nameof(CommentViewModel)}.cshtml", model);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
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
                return Json(new { Status = false, Message = "Error adding comment." });
            }
            
            model.UserId = userId;

            var commentModel = await CommentService.Add(model);

            return Json(new { CommentHashId = commentModel.HashId, Status = true });
        }
    }
}
