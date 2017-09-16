using System;

namespace Stories.Models.Users
{
    public class InviteModel
    {
        public string Email { get; set; }
        public Guid ReferrerUserId { get; set; }
    }
}
