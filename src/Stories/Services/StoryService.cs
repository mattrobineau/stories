using CommonMark;
using Ganss.XSS;
using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Extensions;
using Stories.Models.Stories;
using Stories.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stories.Services
{
    public sealed class StoryService : IStoryService
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IVoteQueueService VoteQueueService;

        public StoryService(IDbContext storiesDbContext, IVoteQueueService voteQueueService)
        {
            StoriesDbContext = storiesDbContext;
            VoteQueueService = voteQueueService;
        }

        public async Task<StorySummaryViewModel> Create(CreateStoryModel model)
        {
            UriBuilder uri = null;

            if (!string.IsNullOrEmpty(model.Url))
            {
                uri = new UriBuilder(model.Url);
                uri.Scheme = "https";
                uri.Port = uri.Port == 80 || uri.Port == 443 ? -1 : uri.Port;
            }

            var story = await StoriesDbContext.Stories.AddAsync(new Story()
            {
                Title = model.Title,
                DescriptionMarkdown = model.DescriptionMarkdown,
                Description = CommonMarkConverter.Convert(model.DescriptionMarkdown),
                Url = uri?.Uri.ToString(),
                UserId = model.UserId,
                UserIsAuthor = model.IsAuthor,
            });

            var rowCount = await StoriesDbContext.SaveChangesAsync();

            var hashIds = new Hashids(minHashLength: 5);

            VoteQueueService.QueueStoryVote(story.Entity.Id);

            return MapToStorySummaryViewModel(story.Entity, hashIds.Encode(story.Entity.Id), model.UserId, false, false);
        }

        public async Task<StoryViewModel> Get(string hashId, Guid? userId)
        {
            var ids = new Hashids(minHashLength: 5);
            var storyId = ids.Decode(hashId).First();

            var story = await StoriesDbContext.Stories.Include(s => s.Comments)
                                                      .SingleOrDefaultAsync(s => s.Id == storyId);

            if (story == null)
                return null;

            var userUpvoted = userId != null && await StoriesDbContext.Votes.AnyAsync(v => story.Id == v.StoryId && v.UserId == userId);
            var userFlagged = userId != null && await StoriesDbContext.Flags.AnyAsync(f => f.UserId == userId && f.StoryId == story.Id);

            var model = new StoryViewModel
            {
                Summary = MapToStorySummaryViewModel(story, hashId, userId, userUpvoted, userFlagged)
            };

            var upvotedComments = await StoriesDbContext.Votes.Where(v => v.CommentId != null && v.UserId == userId && v.Comment.StoryId == story.Id)
                                                              .Select(v => (int)v.CommentId)
                                                              .ToListAsync();

            var flaggedComments = await StoriesDbContext.Flags.Where(f => f.CommentId != null && f.UserId == userId && f.Comment.StoryId == storyId)
                                                              .Select(f => (int)f.CommentId)
                                                              .ToListAsync();

            foreach (var comment in story.Comments.OrderByDescending(c => c.Score?.Value).Where(c => c.ParentCommentId == null))
                model.Comments.Add(MapCommentToCommentViewModel(comment, upvotedComments, flaggedComments));

            return model;
        }

        public async Task<StoriesViewModel> GetTop(int page, int pageSize, Guid? userId)
        {
            return await GetAll(s => s.Score.Value, page, pageSize, userId);
        }

        public async Task<StoriesViewModel> GetNew(int page, int pageSize, Guid? userId)
        {
            return await GetAll(s => Convert.ToDouble(s.CreatedDate.Ticks), page, pageSize, userId);
        }

        private async Task<StoriesViewModel> GetAll(Expression<Func<Story, double>> sort, int page, int pageSize, Guid? userId)
        {
            var model = new StoriesViewModel() { CurrentPage = page, PageSize = pageSize };

            var stories = await StoriesDbContext.Stories.Include(s => s.Comments)
                                                        .Include(s => s.Score)
                                                        .OrderByDescending(s => s.Score != null)
                                                        .ThenBy(sort)
                                                        .ThenByDescending(s => s.CreatedDate)
                                                        .Skip(page * pageSize)
                                                        .Take(pageSize)
                                                        .ToListAsync();
            List<int> upvotedStoryIds = new List<int>();

            if (userId != null)
            {
                upvotedStoryIds = await StoriesDbContext.Votes.Where(v => stories.Any(s => s.Id == v.StoryId) && v.UserId == userId).Select(v => v.StoryId.Value).ToListAsync();
            }

            foreach (var story in stories)
            {
                var userUpvoted = upvotedStoryIds.Any(id => id == story.Id);
                var userFlagged = userId != null && await StoriesDbContext.Flags.AnyAsync(f => f.UserId == userId && f.StoryId == story.Id);
                var hashId = new Hashids(minHashLength: 5).Encode(story.Id);

                model.Stories.Add(MapToStorySummaryViewModel(story, hashId, userId, userUpvoted, userFlagged));
            }

            return model;
        }

        private StorySummaryViewModel MapToStorySummaryViewModel(Story story, string hashId, Guid? userId, bool userUpvoted, bool userFlagged)
        {
            var user = StoriesDbContext.Users.Single(u => u.Id == userId);

            UriBuilder uri = null;

            if (string.IsNullOrEmpty(story.Url))
            {
                var url = $"https://www.dotnetsignals.com/story?hashId={hashId}";
                uri = new UriBuilder(url) { Port = 443 };
            }
            else
            {
                uri = new UriBuilder(story.Url);
            }

            var sanitizer = new HtmlSanitizer();

            return new StorySummaryViewModel()
            {
                HashId = hashId,
                Description = sanitizer.Sanitize(story.Description),
                CommentCount = story.Comments.Count,
                SubmittedDate = story.CreatedDate.ToString("o"),
                Title = sanitizer.Sanitize(story.Title),
                Url = uri.ToString(),
                Hostname = uri.Host,
                Upvotes = story.Upvotes,
                SubmitterUsername = user.IsBanned ? "[banned]" : user.Username,
                Slug = story.Title.ToSlug(),
                IsAuthor = story.UserIsAuthor,
                UserUpvoted = userUpvoted,
                IsSubmitter = userId == story.UserId,
                IsDeletable = DateTime.UtcNow < story.CreatedDate.AddMinutes(5),
                UserFlagged = userFlagged,
            };
        }

        private CommentViewModel MapCommentToCommentViewModel(Comment comment, List<int> upvotedComments, List<int> flaggedComments)
        {
            var model = new CommentViewModel();

            foreach (var reply in comment.Replies.OrderByDescending(c => c?.Score?.Value))
            {
                model.Replies.Add(MapCommentToCommentViewModel(reply, upvotedComments, flaggedComments));
            }

            var hashIds = new Hashids(minHashLength: 5);
            var sanitizer = new HtmlSanitizer();

            model.HashId = hashIds.Encode(comment.Id);
            model.Content = sanitizer.Sanitize(comment.Content);
            model.StoryHashId = hashIds.Encode(comment.Story.Id);
            model.SubmittedDate = comment.CreatedDate.ToString("o");
            model.Username = comment.User.IsBanned ? "[banned]" : comment.User.Username;
            model.Upvotes = comment.Upvotes;
            model.UserUpvoted = upvotedComments.Any(id => id == comment.Id);
            model.UserFlagged = flaggedComments.Any(id => id == comment.Id);

            return model;
        }

        public async Task<bool> Delete(DeleteStoryModel model)
        {
            var story = await StoriesDbContext.Stories.Include(s => s.Comments)
                                                      .Where(s => s.Id == model.StoryId)
                                                      .FirstOrDefaultAsync();

            foreach (var comment in story.Comments)
            {
                StoriesDbContext.Votes.RemoveRange(comment.Votes);
            }

            StoriesDbContext.Comments.RemoveRange(story.Comments);
            StoriesDbContext.Stories.Remove(story);

            return await StoriesDbContext.SaveChangesAsync() > 0;
        }
    }
}
