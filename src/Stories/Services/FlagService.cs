using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Flags;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public sealed class FlagService : IFlagService
    {
        private readonly IDbContext DbContext;

        public FlagService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> ToggleFlag(ToggleFlagModel model)
        {
            var flag = await DbContext.Flags.FirstOrDefaultAsync(f => f.CommentId == model.CommentId && f.StoryId == model.StoryId && f.UserId == model.UserId);

            if(flag == null)
            {
                Create(model);
            }
            else
            {
                Delete(flag);
            }

            return (await DbContext.SaveChangesAsync()) > 0;
        }

        private void Create(ToggleFlagModel model)
        {
            var flag = new Flag
            {
                CommentId = model.CommentId,
                StoryId = model.StoryId,
                UserId = model.UserId
            };

            DbContext.Flags.Add(flag);
        }

        private void Delete(Flag flag)
        {
            if (flag == null)
                return;

            DbContext.Flags.Remove(flag);
        }
    }
}
