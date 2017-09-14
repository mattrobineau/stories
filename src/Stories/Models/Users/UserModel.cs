using System;
using System.Collections.Generic;

namespace Stories.Models.Users
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<RoleModel> Roles { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedDate { get; set; }
        public BanUserModel BanModel { get; set; }
    }
}
