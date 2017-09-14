using System;

namespace Stories.Models.Users
{
    public class BanUserModel
    {
        public Guid BannedByUserId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; }
        public string Reason { get; set; }
        public Guid UserId { get; set; }
    }
}
