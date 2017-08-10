using Stories.Models.Comment;

namespace Stories.Validation.Validators
{
    public class UpdateCommentModelValidator : IValidator<UpdateCommentModel>
    {
        public ValidationResult Validate(UpdateCommentModel instance)
        {
            return new ValidationResult { IsValid = true };
        }
    }
}
