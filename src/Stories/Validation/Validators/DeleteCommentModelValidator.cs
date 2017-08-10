using HashidsNet;
using Stories.Data.DbContexts;
using Stories.Models.Comment;
using System.Linq;

namespace Stories.Validation.Validators
{
    public class DeleteCommentModelValidator : IValidator<DeleteCommentModel>
    {
        private readonly IDbContext DbContext;

        public DeleteCommentModelValidator(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ValidationResult Validate(DeleteCommentModel instance)
        {
            var result = new ValidationResult { IsValid = false };

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
