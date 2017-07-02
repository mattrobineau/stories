using CommonMark;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class CommentService : ICommentService
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IVoteQueueService VoteQueueService;

        public CommentService(IDbContext storiesDbContext, IVoteQueueService voteQueueService)
        {
            StoriesDbContext = storiesDbContext;
            VoteQueueService = voteQueueService;
        }

        public async Task<CommentViewModel> Get(string hashId)
        {
            var hashIds = new Hashids(minHashLength: 5);

            var comment = await StoriesDbContext.Comments.FindAsync(hashIds.Decode(hashId).FirstOrDefault());

            if (comment == null)
                return null;

            return MapCommentToCommentViewModel(comment);
        }

        public async Task<CommentViewModel> Add(AddCommentViewModel model)
        {
            var hashIds = new Hashids(minHashLength: 5);

            var comment = await StoriesDbContext.Comments.AddAsync(new Comment {
                ParentCommentId = string.IsNullOrEmpty(model.ParentCommentHashId) ? null : hashIds.Decode(model.ParentCommentHashId)?.First(),
                ContentMarkdown = model.CommentMarkdown,
                Content = CommonMarkConverter.Convert(model.CommentMarkdown),
                StoryId = hashIds.Decode(model.StoryHashId).First(),
                UserId = model.UserId,
                
            });

            await StoriesDbContext.SaveChangesAsync();

            VoteQueueService.QueueCommentVote(comment.Entity.Id);

            return MapCommentToCommentViewModel(comment.Entity);
        }

        public async Task<bool> Delete(string hashId)
        {
            var commentId = new Hashids(minHashLength: 5).Decode(hashId).FirstOrDefault();

            if (commentId == 0)
                return false;

            var comment = await StoriesDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            comment.IsDeleted = true;

            return await StoriesDbContext.SaveChangesAsync() > 0;
        }

        private CommentViewModel MapCommentToCommentViewModel(Comment comment)
        {
            var hashIds = new Hashids(minHashLength: 5);

            return new CommentViewModel
            {
                Content = comment.IsDeleted ? "<deleted>" : comment.Content,
                HashId = hashIds.Encode(comment.Id),
                StoryHashId = hashIds.Encode(comment.Story.Id),
                SubmittedDate = comment.CreatedDate.ToString("o"),
                Username = comment.User.Username,
                Upvotes = comment.Upvotes,
                IsEdited = comment.IsEdited
            };
        }
    }
}
