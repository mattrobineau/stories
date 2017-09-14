using System.Collections.Generic;

namespace Stories.Models.Users
{
    public class CreateUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
