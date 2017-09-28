using Stories.Configuration;
using Stories.Data.DbContexts;
using Stories.Models.Users;
using Stories.Services;
using Stories.Validation.BusinessRules;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class InviteModelValidator : IValidator<InviteModel>
    {
        private readonly IDbContext DbContext;
        private readonly IEmailRule EmailRule;
        private readonly IUserRules UserRules;
        private readonly InviteOptions Options;
        private readonly IReferralService ReferralService;

        public InviteModelValidator(InviteOptions inviteOptions, IDbContext dbContext, IEmailRule emailRule, IUserRules userRules, IReferralService referralService)
        {
            DbContext = dbContext;
            EmailRule = emailRule;
            UserRules = userRules;
            Options = inviteOptions;
            ReferralService = referralService;
        }

        public ValidationResult Validate(InviteModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrEmpty(instance.Email) || !EmailRule.Validate(instance.Email))
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

            var remainingInvites = Task.Run(async () => await ReferralService.GetRemainingInvites(instance.ReferrerUserId)).Result;

            if (remainingInvites <= 0)
            {
                result.IsValid = false;
                result.Messages.Add("Maximum number of invites has already been reached.");
            }

            return result;
        }
    }
}
