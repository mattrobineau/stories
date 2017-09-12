using Stories.Models.Administration;
using Stories.Validation.BusinessRules;

namespace Stories.Validation.Validators
{
    public class BanUserModelValidator : IValidator<BanUserModel>
    {
        private readonly IUserRules UserRules;

        public BanUserModelValidator(IUserRules userRules)
        {
            UserRules = userRules;
        }

        public ValidationResult Validate(BanUserModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if(!UserRules.UserExists(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("User does not exist.");
            }

            if (string.IsNullOrEmpty(instance.Reason))
            {
                result.IsValid = false;
                result.Messages.Add("A reason must be included with every ban.");
            }

            return result;
        }
    }
}
