using System;

namespace Stories.Models.ViewModels
{
    public class AddCommentViewModel
    {
        public string ParentCommentHashId { get; set; }
        public string StoryHashId { get; set; }
        public string CommentMarkdown { get; set; }
        public Guid UserId { get; set; }
    }
}
