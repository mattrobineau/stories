using System.Collections.Generic;

namespace Stories.Models.ViewModels.User
{
    public class UserViewModel
    {
        public string CreatedDate { get; set; }
        public InviteViewModel InviteViewModel { get; set; }
        public bool IsBanned { get; set; }
        public string BanReason { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}
