using System;

namespace Stories.Models.ViewModels.Administration
{
    public class BanUserViewModel
    {
        public string UserId { get; set; }
        public Guid BannedByUser { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
