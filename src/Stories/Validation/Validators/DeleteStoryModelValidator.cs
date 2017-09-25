using Stories.Constants;
using Stories.Data.DbContexts;
using Stories.Models.Stories;
using Stories.Validation.BusinessRules;
using System.Linq;

namespace Stories.Validation.Validators
{
    public class DeleteStoryModelValidator : IValidator<DeleteStoryModel>
    {
        private readonly IUserRules UserRules;
        private readonly IStoryRules StoryRules;
        private readonly IDbContext DbContext;

        public DeleteStoryModelValidator(IDbContext dbContext, IStoryRules storyRules, IUserRules userRules)
        {
            DbContext = dbContext;
            StoryRules = storyRules;
            UserRules = userRules;
        }

        public ValidationResult Validate(DeleteStoryModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if(!StoryRules.StoryExists(instance.StoryId))
            {
                result.IsValid = false;
                result.Messages.Add("Story does not exist.");
                return result;
            }

            if(!UserRules.UserExists(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("You must be signed in to delete stories.");
                return result;
            }

            if(UserRules.ActionPreventedByUserBan(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("Your account is banned.");
                return result;
            }

            var userHasPrivileges = DbContext.UserRoles.Any(ur => ur.UserId == instance.UserId &&
                                                          (ur.Role.Name == Roles.Admin || ur.Role.Name == Roles.Moderator));

            if(!userHasPrivileges || !StoryRules.UserIsSubmitter(instance.StoryId, instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("You do not have the permissions to delete this story.");
            }

            if(!StoryRules.DeletionThresholdReached(instance.StoryId))
            {
                result.IsValid = false;
                result.Messages.Add("Threshold for story deletion has expired. Moderator intervention is required to delete this story.");
            }

            return result;
        }
    }
}
