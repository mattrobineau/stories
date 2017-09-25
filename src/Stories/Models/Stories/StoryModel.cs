using System;

namespace Stories.Models.Stories
{
    public class StoryModel
    {
        public int CommentCount { get; set; }
        public string Description { get; set; }
        public string Hostname { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string SubmitterUsername { get; set; }
        public int Upvotes { get; set; }
        public string Url { get; set; }
        public bool UserUpvoted { get; set; }
    }
}
