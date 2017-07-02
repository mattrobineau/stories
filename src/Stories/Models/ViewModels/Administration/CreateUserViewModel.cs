using System.Collections.Generic;

namespace Stories.Models.ViewModels.Administration
{
    public class CreateUserViewModel : SignupViewModel
    {
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
