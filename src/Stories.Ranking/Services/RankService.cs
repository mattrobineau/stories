using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Data.Sorting;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Ranking.Services
{
    public sealed class RankService : IRankService
    {
        private readonly IDbContext dbContext;

        public async Task<bool> CalculateStoryScore(int id)
        {
            var story = await dbContext.Stories.Include(s => s.Score)
                                               .Where(s => s.Id == id)
                                               .FirstOrDefaultAsync();

            CalculateStoryScore(story);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public void CalculateStoryScore(Story story)
        {
            if (story == null)
                return;

            if (story.Score == null)
            {
                story.Score = new StoryScore { StoryId = story.Id };
            }

            story.Score.Value = Rank.Hot(story.Upvotes, 0, story.CreatedDate);
        }

        public async Task<bool> CalculateCommentScore(int id)
        {
            var comment = await dbContext.Comments.Include(c => c.Score)
                                                  .Where(c => c.Id == id)
                                                  .FirstOrDefaultAsync();

            CalculateCommentScore(comment);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public void CalculateCommentScore(Comment comment)
        {
            if (comment == null)
                return;

            if (comment.Score == null)
            {
                comment.Score = new CommentScore { CommentId = comment.Id };
            }

            comment.Score.Value = Rank.Confidence(comment.Upvotes, 0);
        }
    }
}
