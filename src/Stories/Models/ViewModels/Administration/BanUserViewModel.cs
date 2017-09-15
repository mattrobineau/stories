using System;
using System.ComponentModel.DataAnnotations;

namespace Stories.Models.ViewModels.Administration
{
    public class BanUserViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }
    }
}
