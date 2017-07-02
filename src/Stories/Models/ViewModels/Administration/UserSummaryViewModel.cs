using System;

namespace Stories.Models.ViewModels.Administration
{
    public class UserSummaryViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsBanned { get; set; }
        public bool IsModerator { get; set; }
    }
}
