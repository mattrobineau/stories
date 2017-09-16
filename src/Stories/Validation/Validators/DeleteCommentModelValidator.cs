using HashidsNet;
using Stories.Data.DbContexts;
using Stories.Models.Comment;
using Stories.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class DeleteCommentModelValidator : IValidator<DeleteCommentModel>
    {
        private readonly IDbContext DbContext;
        private readonly IUserService UserService;

        public DeleteCommentModelValidator(IDbContext dbContext, IUserService userService)
        {
            DbContext = dbContext;
            UserService = userService;
        }

        public ValidationResult Validate(DeleteCommentModel instance)
        {
            var result = new ValidationResult { IsValid = false };

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

            var hashIds = new Hashids(minHashLength: 5);

            var commentId = hashIds.Decode(instance.HashId).FirstOrDefault();

            if (commentId == 0)
            {
                result.Messages.Add("Invalid comment id.");
                return result;
            }

            var comment = DbContext.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                result.Messages.Add("Invalid comment id.");
                return result;
            }

            if(comment.UserId != instance.UserId)
            {
                result.Messages.Add("You are not the owner of this comment.");
                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}
