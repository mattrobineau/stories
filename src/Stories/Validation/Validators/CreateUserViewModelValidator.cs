using Stories.Models.ViewModels;
using Stories.Models.ViewModels.Administration;
using System.Linq;

namespace Stories.Validation.Validators
{
    public class CreateUserViewModelValidator : IValidator<CreateUserViewModel>
    {

        private readonly IValidator<SignupViewModel> SignupViewModelValidator;

        public CreateUserViewModelValidator(IValidator<SignupViewModel> signupViewModelValidator)
        {
            SignupViewModelValidator = signupViewModelValidator;
        }

        public ValidationResult Validate(CreateUserViewModel instance)
        {
            var result = SignupViewModelValidator.Validate((SignupViewModel)instance);

            if (!instance.Roles.Any())
            {
                result.IsValid = false;
                result.Messages.Add("A user must have at least 1 role.");
            }            

            return result;
        }
    }
}
