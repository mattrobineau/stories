using CommonMark;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Comment;
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

            return await MapCommentToCommentViewModel(comment);
        }

        public async Task<CommentViewModel> Add(AddCommentViewModel model)
        {
            var hashIds = new Hashids(minHashLength: 5);

            var comment = await StoriesDbContext.Comments.AddAsync(new Comment {
                ParentCommentId = string.IsNullOrEmpty(model.ParentCommentHashId) ? null : hashIds.Decode(model.ParentCommentHashId)?.First(),
                ContentMarkdown = model.CommentMarkdown,
                Content = CommonMarkConverter.Convert(model.CommentMarkdown),
                StoryId = hashIds.Decode(model.StoryHashId).First(),
                UserId = model.UserId
            });

            await StoriesDbContext.SaveChangesAsync();

            VoteQueueService.QueueCommentVote(comment.Entity.Id);

            return await MapCommentToCommentViewModel(comment.Entity);
        }

        public async Task<bool> Delete(DeleteCommentModel model)
        {
            var commentId = new Hashids(minHashLength: 5).Decode(model.HashId).FirstOrDefault();

            if (commentId == 0)
                return false;

            var comment = await StoriesDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            comment.IsDeleted = true;

            return await StoriesDbContext.SaveChangesAsync() > 0;
        }

        public async Task<CommentViewModel> Update(UpdateCommentModel model)
        {
            return new CommentViewModel();
        }

        private async Task<CommentViewModel> MapCommentToCommentViewModel(Comment comment)
        {
            var hashIds = new Hashids(minHashLength: 5);
            var username = await StoriesDbContext.Users.Where(u => u.Id == comment.UserId).Select(u => u.Username).FirstOrDefaultAsync();

            return new CommentViewModel
            {
                Content = comment.IsDeleted ? "<deleted>" : comment.Content,
                HashId = hashIds.Encode(comment.Id),
                StoryHashId = hashIds.Encode(comment.StoryId),
                SubmittedDate = comment.CreatedDate.ToString("o"),
                Username = username,
                Upvotes = comment.Upvotes,
                IsEdited = comment.IsEdited
            };
        }
    }
}
