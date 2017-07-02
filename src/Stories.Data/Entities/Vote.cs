using System;

namespace Stories.Data.Entities
{
    public class Vote : ITimestamp
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public int? CommentId { get; set; }
        public int? StoryId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Story Story { get; set; }
        public virtual User User { get; set; }

        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }
}
