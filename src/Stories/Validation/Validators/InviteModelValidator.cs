using Stories.Data.DbContexts;
using Stories.Models.Users;
using Stories.Validation.BusinessRules;
using System.Linq;

namespace Stories.Validation.Validators
{
    public class InviteModelValidator : IValidator<InviteModel>
    {
        private readonly IDbContext DbContext;
        private readonly IEmailRule EmailRule;
        private readonly IUserRules UserRules;

        public InviteModelValidator(IDbContext dbContext, IEmailRule emailRule, IUserRules userRules)
        {
            DbContext = dbContext;
            EmailRule = emailRule;
            UserRules = userRules;
        }

        public ValidationResult Validate(InviteModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (!EmailRule.Validate(instance.Email))
            {
                result.IsValid = false;
                result.Messages.Add("Invalid email address.");
            }

            if (!UserRules.UserExists(instance.ReferrerUserId))
            {
                result.IsValid = false;
                result.Messages.Add("You must be signed in to invite users.");
                return result;
            }

            var referralCount = DbContext.Referrals.Where(r => r.ReferrerUserId == instance.ReferrerUserId).Count();

            //TODO move limit to appsettings.json
            if (referralCount >= 5)
            {
                result.IsValid = false;
                result.Messages.Add("Maximum number of invites has already been reached.");
            }

            return result;
        }
    }
}
