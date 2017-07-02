using System.Collections.Generic;

namespace Stories.Models.ViewModels.User
{
    public class UserViewModel
    {
        public string CreatedDate { get; set; }
        public bool IsBanned { get; set; }
        public string BanReason { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public List<StorySummaryViewModel> RecentStories { get; set; } = new List<StorySummaryViewModel>();
        public List<CommentViewModel> RecentComments { get; set; } = new List<CommentViewModel>();
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}
