using Stories.Models.Comment;
using Stories.Services;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class UpdateCommentModelValidator : IValidator<UpdateCommentModel>
    {
        private readonly IUserService UserService;

        public UpdateCommentModelValidator(IUserService userService)
        {
            UserService = userService;
        }

        public ValidationResult Validate(UpdateCommentModel instance)
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
                result.Messages.Add("You account is banned.");
                return result;
            }

            return result;
        }
    }
}
