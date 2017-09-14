using System;

namespace Stories.Models.Users
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public Guid UserId { get; set; }
    }
}
