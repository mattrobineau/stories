using Stories.Constants;
using Stories.Data.DbContexts;
using Stories.Models.Users;
using Stories.Services;
using Stories.Validation.BusinessRules;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class BanUserModelValidator : IValidator<BanUserModel>
    {
        private readonly IUserRules UserRules;
        private readonly IUserService UserService;

        public BanUserModelValidator(IUserRules userRules, IDbContext dbContext, IUserService userService)
        {
            UserRules = userRules;
            UserService = userService;
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

            var user = Task.Run(async () => await UserService.GetUser(instance.BannedByUserId)).Result;
            var bannedUser = Task.Run(async () => await UserService.GetUser(instance.UserId)).Result;

            if(!user.Roles.Any(r => r.Name == Roles.Admin) && bannedUser.Roles.Any(r => r.Name == Roles.Moderator || r.Name == Roles.Admin))
            {
                result.IsValid = false;
                result.Messages.Add("You do not have sufficient privileges to ban this user.");
            }

            return result;
        }

        
    }
}
