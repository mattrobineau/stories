using System;
using System.Collections.Generic;

namespace Stories.Data.Entities
{
    public class Comment : ITimestamp
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public string ContentMarkdown { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }
        public int? ParentCommentId { get; set; }
        public int? ScoreId { get; set; }
        public int StoryId { get; set; }
        public int Upvotes { get; set; }
        public Guid UserId { get; set; }

        public virtual Comment ParentComment { get; set; }
        public virtual IList<Comment> Replies { get; set; } = new List<Comment>();
        public virtual IList<Flag> Flags { get; set; } = new List<Flag>();
        public virtual CommentScore Score { get; set; }
        public virtual Story Story { get; set; }
        public virtual User User { get; set; }
        public virtual IList<Vote> Votes { get; set; } = new List<Vote>();

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }
}
