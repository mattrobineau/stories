using System;

namespace Stories.Data.Entities
{
    public class Referral : ITimestamp
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public Guid ReferrerUserId { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual User Referrer { get; set; }
    }
}
