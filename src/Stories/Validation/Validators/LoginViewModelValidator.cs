using Stories.Models.ViewModels;
using Stories.Validation.BusinessRules;

namespace Stories.Validation.Validators
{
    public class LoginViewModelValidator : IValidator<LoginViewModel>
    {
        private readonly IEmailRule EmailBusinessRule;
        private readonly IUserRules UserRules;

        public LoginViewModelValidator(IEmailRule emailBusinessRule, IUserRules userRules)
        {
            EmailBusinessRule = emailBusinessRule;
            UserRules = userRules;
        }

        public ValidationResult Validate(LoginViewModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(instance.Password))
            {
                result.IsValid = false;
                result.Messages.Add("Password field cannot be empty.");
            }

            if (string.IsNullOrEmpty(instance.Email) || !EmailBusinessRule.Validate(instance.Email))
            {
                result.IsValid = false;
                result.Messages.Add("Invalid email address.");
            }

            if(UserRules.ActionPreventedByUserBan(instance.Email))
            {
                result.IsValid = false;
                result.Messages.Add("This account has been banned.");
            }

            return result;
        }
    }
}
