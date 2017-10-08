using System.Collections.Generic;

namespace Stories.Models.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            Replies = new List<CommentViewModel>();
        }

        public string HashId { get; set; }
        public string Content { get; set; }
        public string StoryHashId { get; set; }
        public bool IsEdited { get; set; }
        public List<CommentViewModel> Replies { get; set; }
        public string SubmittedDate { get; set; }
        public int Upvotes { get; set; }
        public string Username { get; set; }
        public bool UserUpvoted { get; set; }
        public bool UserFlagged { get; set; }
    }
}
