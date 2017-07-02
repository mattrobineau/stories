namespace Stories.Models.ViewModels.User
{
    public class ReferralViewModel : SignupViewModel
    {
        public bool CodeIsValid { get; set; }
        public string Code { get; set; }
    }
}
