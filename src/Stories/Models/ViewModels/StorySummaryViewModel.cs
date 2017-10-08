namespace Stories.Models.ViewModels
{
    public class StorySummaryViewModel
    {
        public string HashId { get; set; }
        public int CommentCount { get; set; }
        public string Description { get; set; }
        public bool UserUpvoted { get; set; }
        public string SubmittedDate { get; set; }
        public string SubmitterUsername { get; set; }
        public string Title { get; set; }
        public int Upvotes { get; set; }
        public bool UserFlagged { get; set; }
        public string Url { get; set; }
        public string Hostname { get; set; }
        public string Slug { get; set; }
        public string Username { get; set; }
        public bool IsAuthor { get; set; }
        public bool IsSubmitter { get; set; }
        public bool IsDeletable { get; set; }        
    }
}
