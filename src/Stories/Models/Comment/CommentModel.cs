using System.Collections.Generic;

namespace Stories.Models.Comment
{
    public class CommentModel
    {
        public CommentModel()
        {
            Replies = new List<CommentModel>();
        }

        public string Id { get; set; }
        public string Content { get; set; }
        public string StoryId { get; set; }
        public bool IsEdited { get; set; }
        public List<CommentModel> Replies { get; set; }
        public string SubmittedDate { get; set; }
        public int Upvotes { get; set; }
        public string Username { get; set; }
        public bool UserUpvoted { get; set; }
        public bool UserFlagged { get; set; }
    }
}
