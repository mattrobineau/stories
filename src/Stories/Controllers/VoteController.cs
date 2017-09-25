using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Models.Votes;
using Stories.Services;
using Stories.Validation.Validators;
using System;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class VoteController : BaseController
    {
        private readonly IVoteService VoteService;
        private readonly IValidator<ToggleVoteModel> ToggleVoteModelValidator;

        public VoteController(IVoteService voteService, IValidator<ToggleVoteModel> toggleVoteModelValidator)
        {
            VoteService = voteService;
            ToggleVoteModelValidator = toggleVoteModelValidator;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> StoryVote(string hashId)
        {
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Message = "Error upvoting." });
            }

            var toggleVoteModel = new ToggleVoteModel { HashId = hashId, UserId = userId };
            var validationResult = ToggleVoteModelValidator.Validate(toggleVoteModel);

            if(!validationResult.IsValid)
            {
                return Json(new { status = false, messages = validationResult.Messages });
            }

            if (await VoteService.ToggleStoryVote(toggleVoteModel))
                return Json(new { Status = true });

            return Json(new { Status = false });
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CommentVote(string hashId)
        {
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Message = "Error upvoting." });
            }

            var toggleVoteModel = new ToggleVoteModel { HashId = hashId, UserId = userId };
            var validationResult = ToggleVoteModelValidator.Validate(toggleVoteModel);

            if (!validationResult.IsValid)
            {
                return Json(new { status = false, messages = validationResult.Messages });
            }

            if (await VoteService.ToggleCommentVote(toggleVoteModel))
                return Json(new { Status = true });

            return Json(new { Status = false });
        }
    }
}