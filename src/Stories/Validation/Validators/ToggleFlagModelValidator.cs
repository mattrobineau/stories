using Stories.Models.Flags;
using Stories.Validation.BusinessRules;

namespace Stories.Validation.Validators
{
    public class ToggleFlagModelValidator : IValidator<ToggleFlagModel>
    {
        private readonly IUserRules UserRules;

        public ToggleFlagModelValidator(IUserRules userRules)
        {
            UserRules = userRules;
        }

        public ValidationResult Validate(ToggleFlagModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (instance.CommentId == null && instance.StoryId == null)
            {
                result.IsValid = false;
                result.Messages.Add("No comment or story was provided.");
            }

            if (instance.CommentId != null && instance.StoryId != null)
            {
                result.IsValid = false;
                result.Messages.Add("Invalid flagging selection.");
            }

            if (!UserRules.UserExists(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("Please sign in to flag a story or comment.");
            }

            return result;
        }
    }
}
