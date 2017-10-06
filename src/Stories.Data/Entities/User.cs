using System;
using System.Collections.Generic;

namespace Stories.Data.Entities
{
    public class User : ITimestamp
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid EmailVerificationCode { get; set; }
        public bool IsEmailVerified { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsBanned { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? SettingsId { get; set; }

        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
        public virtual IList<Flag> Flags { get; set; } = new List<Flag>();
        public virtual IList<Story> Stories { get; set; } = new List<Story>();
        public virtual IList<UserRole> Roles { get; set; } = new List<UserRole>();
        public virtual IList<Vote> Votes { get; set; } = new List<Vote>();
        public virtual IList<Referral> Referrals { get; set; } = new List<Referral>();
        public virtual UserSettings Settings { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }
}
