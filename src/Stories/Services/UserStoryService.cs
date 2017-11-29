using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Stories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IDbContext DbContext;

        public UserStoryService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<StoryModel>> GetRecent(Guid forUserId, Guid callingUserId, int page, int pageSize, bool includeDeleted = false)
        {
            var models = new List<StoryModel>();

            var stories = await DbContext.Stories.Where(s => s.UserId == forUserId && s.IsDeleted == includeDeleted)
                                                 .Include(s => s.Comments)
                                                 .Include(s => s.Score)
                                                 .Include(s => s.Flags)
                                                 .OrderByDescending(s => s.CreatedDate)
                                                 .Skip(page * pageSize)
                                                 .Take(pageSize)
                                                 .ToListAsync();

            return await MapStoriesToModel(stories, forUserId, callingUserId);

        }

        private async Task<List<StoryModel>> MapStoriesToModel(List<Story> stories, Guid forUserId, Guid callingUserId)
        {
            var models = new List<StoryModel>();

            if (!stories.Any())
            {
                return models;
            }

            var upvotedStories = await DbContext.Votes.Where(v => stories.Any(s => s.Id == v.StoryId) && v.UserId == callingUserId)
                                                     .Select(v => v.StoryId.Value).ToArrayAsync();

            var submitterUsername = await DbContext.Users.Where(u => u.Id == forUserId)
                                                         .Select(u => u.Username)
                                                         .FirstOrDefaultAsync();

            foreach (var story in stories)
            {
                UriBuilder uri = null;
                if (!string.IsNullOrEmpty(story.Url))
                {
                    uri = new UriBuilder(story.Url);
                }

                var model = new StoryModel
                {
                    CommentCount = story.Comments.Count(),
                    Description = story.Description,
                    Flags = story.Flags.Count(),
                    Hostname = uri.Host,
                    Id = story.Id,
                    SubmittedDate = story.CreatedDate,
                    SubmitterUsername = submitterUsername,
                    Title = story.Title,
                    Upvotes = story.Upvotes,
                    Url = uri?.ToString(),
                    UserFlagged = story.Flags.Any(f => f.UserId == callingUserId),
                    UserUpvoted = upvotedStories.Contains(story.Id)
                };

                models.Add(model);
            }

            return models;
        }
    }
}
