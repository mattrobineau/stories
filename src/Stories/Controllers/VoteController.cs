using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stories.Services;
using System;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class VoteController : BaseController
    {
        private readonly IVoteService VoteService;

        public VoteController(IVoteService voteService)
        {
            VoteService = voteService;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> StoryVote(string hashId)
        {
            if (!Guid.TryParse(CurrentUser.NameIdentifier, out Guid userId))
            {
                return Json(new { Status = false, Message = "Error upvoting." });
            }

            if (await VoteService.ToggleStoryVote(hashId, userId))
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

            if (await VoteService.ToggleCommentVote(hashId, userId))
                return Json(new { Status = true });

            return Json(new { Status = false });
        }
    }
}