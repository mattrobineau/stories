using System;

namespace Stories.Models.User
{
    public class UserBanModel
    {
        public Guid UserId { get; set; }
        public string BanReason { get; set; }
        public bool IsBanned { get; set; }
    }
}
