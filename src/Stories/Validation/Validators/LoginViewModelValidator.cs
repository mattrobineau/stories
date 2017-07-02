using Stories.Models.ViewModels;
using Stories.Validation.BusinessRules;

namespace Stories.Validation.Validators
{
    public class LoginViewModelValidator : IValidator<LoginViewModel>
    {
        private readonly IEmailRule EmailBusinessRule;

        public LoginViewModelValidator(IEmailRule emailBusinessRule)
        {
            EmailBusinessRule = emailBusinessRule;
        }

        public ValidationResult Validate(LoginViewModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(instance.Password))
            {
                result.IsValid = false;
                result.Messages.Add("Password field cannot be empty.");
            }

            if (!EmailBusinessRule.Validate(instance.Email))
            {
                result.IsValid = false;
                result.Messages.Add("Invalid email address.");
            }

            return result;
        }
    }
}
