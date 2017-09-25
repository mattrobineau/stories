using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Validation.BusinessRules
{
    public class StoryRules : IStoryRules
    {
        private readonly IDbContext DbContext;
        
        public StoryRules(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool StoryExists(int storyId)
        {
            return Task.Run(async () => { return await DbContext.Stories.AnyAsync(s => s.Id == storyId); }).Result;
        }

        public bool UserIsSubmitter(int storyId, Guid userId)
        {
            return Task.Run(async () => { return await DbContext.Stories.AnyAsync(s => s.Id == storyId && s.UserId == userId); }).Result;
        }

        public bool DeletionThresholdReached(int storyId)
        {
            var createdDate = Task.Run(async () => { return await DbContext.Stories.Where(s => s.Id == storyId).Select(s => s.CreatedDate).SingleAsync(); }).Result;

            return DeletionThresholdReached(createdDate);
        }

        public bool DeletionThresholdReached(DateTime createdDate)
        {
            return DateTime.UtcNow < createdDate.AddMinutes(5);
        }
    }
}
