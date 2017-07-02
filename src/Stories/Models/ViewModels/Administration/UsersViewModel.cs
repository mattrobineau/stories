using System.Collections.Generic;

namespace Stories.Models.ViewModels.Administration
{
    public class UsersViewModel
    {
        public List<UserSummaryViewModel> Users { get; set; } = new List<UserSummaryViewModel>();
    }
}
