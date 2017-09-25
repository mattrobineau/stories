using Stories.Models.Stories;
using Stories.Services;
using System;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class CreateStoryModelValidator : IValidator<CreateStoryModel>
    {
        private readonly IUserService UserService;

        public CreateStoryModelValidator(IUserService userService)
        {
            UserService = userService;
        }

        public ValidationResult Validate(CreateStoryModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            var user = Task.Run(async () => await UserService.GetUser(instance.UserId)).Result;

            if (user == null)
            {
                result.IsValid = false;
                result.Messages.Add("Error: Please sign in again.");
            }

            if (user.IsBanned)
            {
                result.IsValid = false;
                result.Messages.Add("Your account is banned.");
                return result;
            }

            if (string.IsNullOrEmpty(instance.Title))
            {
                result.IsValid = false;
                result.Messages.Add("Story must have a title.");
            }

            if (!string.IsNullOrEmpty(instance.Url) && !Uri.IsWellFormedUriString(instance.Url, UriKind.Absolute))
            {
                result.IsValid = false;
                result.Messages.Add("The story url is invalid.");
            }

            return result;
        }
    }
}
