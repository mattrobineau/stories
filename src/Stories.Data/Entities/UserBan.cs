using System;

namespace Stories.Data.Entities
{
    public class UserBan : ITimestamp
    {
        public Guid BannedByUserId { get; set; }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Notes { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Reason { get; set; }

        public virtual User BannedByUser { get; set; }
        public virtual User User { get; set; }
    }
}
