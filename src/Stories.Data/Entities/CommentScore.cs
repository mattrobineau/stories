using System;

namespace Stories.Data.Entities
{
    public class CommentScore : ITimestamp
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public double Value { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
