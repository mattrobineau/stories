using Stories.Models.ViewModels;
using Stories.Services;
using Stories.Validation.BusinessRules;
using System.Text.RegularExpressions;

namespace Stories.Validation.Validators
{
    public class SignupViewModelValidator : IValidator<SignupViewModel>
    {
        private readonly IUserService UserService;
        private readonly IEmailRule EmailBusinessRule;

        public SignupViewModelValidator(IUserService userService, IEmailRule emailBusinessRule)
        {
            UserService = userService;
            EmailBusinessRule = emailBusinessRule;
        }

        public ValidationResult Validate(SignupViewModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (instance.ConfirmPassword != instance.Password)
            {
                result.IsValid = false;
                result.Messages.Add("Password confirmation does not match password.");
            }

            // Username validation
            if (UserService.UsernameAvailable(instance.Username).Result)
            {
                result.IsValid = false;
                result.Messages.Add("Username is not available.");
            }

            var matched = Regex.IsMatch(instance.Username, @"^[a-zA-Z]+([0-9A-Za-z|_|\.|\-]+)?$");
            if (!matched)
            {
                result.IsValid = false;
                result.Messages.Add("Username is not valid.");
            }

            if(!EmailBusinessRule.Validate(instance.Email))
            {
                result.IsValid = false;
                result.Messages.Add("Invalid email address.");
            }

            return result;
        }
    }
}
