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

        public async Task<List<StoryModel>> GetRecent(Guid userId)
        {
            var stories = await DbContext.Stories.Where(s => s.UserId == userId && !s.IsDeleted)
                                                 .OrderByDescending(s => s.CreatedDate)
                                                 .Take(5)
                                                 .Include(s => s.User)
                                                 .ToListAsync();

           return MapStoryToModel(stories);
        }

        public async Task<List<StoryModel>> GetAll(Guid userId, bool includeDeleted)
        {
            var stories = await DbContext.Stories.Where(s => s.UserId == userId)
                                                 .OrderByDescending(s => s.CreatedDate)
                                                 .Include(s => s.User)
                                                 .ToListAsync();

           return MapStoryToModel(stories);
            
        }

        private List<StoryModel> MapStoryToModel(List<Story> stories)
        {
            var models = new List<StoryModel>();

            foreach(var story in stories)
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
                    Hostname = uri.Host,
                    Id = story.Id,
                    SubmittedDate = story.CreatedDate,
                    Title = story.Title,
                    Upvotes = story.Upvotes,
                    Url = uri?.ToString(),
                    UserUpvoted = false
                };

                models.Add(model);
            }

            return models;
        }
    }
}
