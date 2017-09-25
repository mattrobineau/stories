using System;

namespace Stories.Models.Stories
{
    public class DeleteStoryModel
    {
        public int StoryId { get; set; }
        public Guid UserId { get; set; }
    }
}
