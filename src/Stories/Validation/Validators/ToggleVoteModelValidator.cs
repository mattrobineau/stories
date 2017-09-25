using Stories.Models.Votes;
using Stories.Validation.BusinessRules;

namespace Stories.Validation.Validators
{
    public class ToggleVoteModelValidator : IValidator<ToggleVoteModel>
    {
        private readonly IUserRules UserRules;

        public ToggleVoteModelValidator(IUserRules userRules)
        {
            UserRules = userRules;
        }

        public ValidationResult Validate(ToggleVoteModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if(UserRules.ActionPreventedByUserBan(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("This account has been banned.");
            }

            return result;
        }
    }
}
