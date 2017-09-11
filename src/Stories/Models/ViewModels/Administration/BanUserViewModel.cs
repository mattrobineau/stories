using System;

namespace Stories.Models.ViewModels.Administration
{
    public class BanUserViewModel
    {
        public Guid UserId { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
