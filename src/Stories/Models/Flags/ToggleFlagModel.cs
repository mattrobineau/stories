using System;

namespace Stories.Models.Flags
{
    public class ToggleFlagModel
    {
        public int? StoryId { get; set; }
        public int? CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
