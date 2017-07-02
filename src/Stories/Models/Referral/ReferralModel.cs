using System;

namespace Stories.Models.Referral
{
    public class ReferralModel
    {
        public Guid Code { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
