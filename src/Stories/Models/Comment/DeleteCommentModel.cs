using System;

namespace Stories.Models.Comment
{
    public class DeleteCommentModel
    {
        public Guid UserId { get; set; }
        public string HashId { get; set; }
    }
}
