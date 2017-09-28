using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Models.Users;
using Stories.Services;
using Stories.Validation.BusinessRules;
using System.Linq;

namespace Stories.Validation.Validators
{
    public class ChangePasswordModelValidator : IValidator<ChangePasswordModel> 
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IPasswordService PasswordService;
        private readonly IUserRules UserRules;

        public ChangePasswordModelValidator(IDbContext storiesDbContext, IPasswordService passwordService, IUserRules userRules)
        {
            StoriesDbContext = storiesDbContext;
            PasswordService = passwordService;
            UserRules = userRules;
        }

        public ValidationResult Validate(ChangePasswordModel instance)
        {
            var result = new ValidationResult { IsValid = false };

            if(!UserRules.UserExists(instance.UserId))
            {
                result.IsValid = false;
                result.Messages.Add("User does not exist.");
                return result;
            }

            if(string.IsNullOrEmpty(instance.NewPassword))
            {
                result.IsValid = false;
                result.Messages.Add("New password cannot be empty.");
                return result;
            }

            if(string.IsNullOrEmpty(instance.OldPassword))
            {
                result.IsValid = false;
                result.Messages.Add("Current password cannot be empty.");
                return result;
            }
                        
            if (instance.ConfirmNewPassword != instance.NewPassword)
            {
                result.IsValid = false;
                result.Messages.Add("Password confirmation does not match password.");
            }

            if (instance.NewPassword == instance.OldPassword)
            {
                result.IsValid = false;
                result.Messages.Add("New and old password are the same.");
            }

            var user = StoriesDbContext.Users.Where(u => u.Id.Equals(instance.UserId)).Include(u => u.Roles).ThenInclude(r => r.Role).FirstOrDefault();

            if (!PasswordService.VerifyUserPassword(instance.OldPassword, user?.PasswordHash, user?.PasswordSalt))
            {
                result.IsValid = false;
                result.Messages.Add("Current password is invalid.");
            }

            return result;
        }
    }
}
