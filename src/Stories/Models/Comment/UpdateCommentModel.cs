using System;

namespace Stories.Models.Comment
{
    public class UpdateCommentModel
    {
        public string HashId { get; set; }
        public Guid UserId { get; set; }
        public string CommentMarkdown { get; set; }
    }
}
