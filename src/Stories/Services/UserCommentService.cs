using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class UserCommentService
    {
        private readonly IDbContext dbContext;

        public UserCommentService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IList<CommentModel>> GetRecent(Guid forUserId, Guid callingUserId, int page, int pageSize, bool includeDeleted = false)
        {
            var comments = await dbContext.Comments.Include(c => c.Flags)
                                                   .Where(c => c.UserId == forUserId && c.IsDeleted == includeDeleted)
                                                   .OrderByDescending(c => c.CreatedDate)
                                                   .Skip(page * pageSize)
                                                   .Take(pageSize)
                                                   .ToListAsync();
            return null;
        }

        private async Task<IList<CommentModel>> MapCommentsToModels(IList<Comment> comments, Guid forUserId, Guid callingUserId)
        {
            var models = new List<CommentModel>();

            if(!comments.Any())
            {
                return models;
            }

            var submitterUsername = await dbContext.Users.Where(u => u.Id == forUserId)
                                                         .Select(u => u.Username)
                                                         .FirstOrDefaultAsync();

            var upvotedCommentIds = await dbContext.Votes.Where(v => v.CommentId != null && v.UserId == callingUserId)
                                                         .Select(v => v.CommentId.Value)
                                                         .ToArrayAsync();

            foreach(var comment in comments)
            {
                var model = new CommentModel
                {
                    Content = comment.Content,
                    Flags = comment.Flags.Count(),
                    Id = comment.Id,
                    IsEdited = comment.IsEdited,
                    StoryId = comment.StoryId,
                    SubmittedDate = comment.CreatedDate,
                    SubmittedUsername = submitterUsername,
                    Upvotes = comment.Upvotes,
                    UserFlagged = comment.Flags.Any(f => f.UserId == callingUserId),
                    UserUpvoted = upvotedCommentIds.Contains(comment.Id)
                };

                models.Add(model);
            }

            return models;
        }
    }
}
