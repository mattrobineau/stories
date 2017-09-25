using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Votes;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class VoteService : IVoteService
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IVoteQueueService VoteQueueService;

        public VoteService(IDbContext storiesDbContext, IVoteQueueService voteQueueService)
        {
            StoriesDbContext = storiesDbContext;
            VoteQueueService = voteQueueService;
        }

        public async Task<bool> ToggleStoryVote(ToggleVoteModel model)
        {
            var storyId = new Hashids(minHashLength: 5).Decode(model.HashId)?.FirstOrDefault();

            if (storyId == 0)
                return false;

            var story = await StoriesDbContext.Stories.Include(v => v.Votes).Where(o => o.Id == storyId).FirstOrDefaultAsync();

            if(story == null)
                return false;
                        
            var vote = story.Votes.FirstOrDefault(vo => vo.UserId == model.UserId);

            if (vote != null)
            {
                story.Upvotes--;

                return await DeleteVote(vote) > 0;
            }

            vote = new Vote
            {
                StoryId = storyId,
                UserId = model.UserId,
            };

            story.Upvotes++;

            VoteQueueService.QueueStoryVote(story.Id);

            return await AddVote(vote) > 0;
        }

        public async Task<bool> ToggleCommentVote(ToggleVoteModel model)
        {
            var commentId = new Hashids(minHashLength: 5).Decode(model.HashId)?.FirstOrDefault();

            if (commentId == 0)
                return false;

            var comment = await StoriesDbContext.Comments.Include(v => v.Votes).Where(o => o.Id == commentId).FirstOrDefaultAsync();

            if (comment == null)
                return false;

            var vote = comment.Votes.FirstOrDefault(v => v.UserId == model.UserId);

            if (vote != null)
            {
                comment.Upvotes--;
                return await DeleteVote(vote) > 0;
            }

            vote = new Vote
            {
                CommentId = commentId,
                UserId = model.UserId,
            };

            comment.Upvotes++;

            VoteQueueService.QueueCommentVote(comment.Id);

            return await AddVote(vote) > 0;
        }

        private async Task<int> DeleteVote(Vote vote)
        {
            if (vote == null)
                return 0;

            StoriesDbContext.Votes.Remove(vote);

            return await StoriesDbContext.SaveChangesAsync();
        }

        private async Task<int> AddVote(Vote vote)
        {
            if (vote == null)
                return 0;

            await StoriesDbContext.Votes.AddAsync(vote);

            return await StoriesDbContext.SaveChangesAsync();
        }
    }
}
