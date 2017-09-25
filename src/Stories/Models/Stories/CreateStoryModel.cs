using System;

namespace Stories.Models.Stories
{
    public class CreateStoryModel
    {
        public int? Id { get; set; }
        public bool IsAuthor { get; set; }
        public string Title { get; set; }
        public string DescriptionMarkdown { get; set; }
        public string Url { get; set; }
        public Guid UserId { get; set; }
    }
}
