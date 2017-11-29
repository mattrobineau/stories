using System;

namespace Stories.Models.Comment
{
    public class CommentModel
    {
        public string Content { get; set; }
        public int Flags { get; set; }
        public int Id { get; set; }
        public bool IsEdited { get; set; }
        public int StoryId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string SubmittedUsername { get; set; }
        public int Upvotes { get; set; }
        public bool UserFlagged { get; set; }
        public bool UserUpvoted { get; set; }
    }
}
