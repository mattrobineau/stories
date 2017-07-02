using Stories.Models.ViewModels;

namespace Stories.Validation.Validators
{
    public class AddCommentViewModelValidator : IValidator<AddCommentViewModel>
    {
        public ValidationResult Validate(AddCommentViewModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if(string.IsNullOrEmpty(instance.CommentMarkdown))
            {
                result.IsValid = false;
                result.Messages.Add("Comment body cannot be empty.");
            }

            if(string.IsNullOrEmpty(instance.StoryHashId))
            {
                result.IsValid = false;
                result.Messages.Add("Cannot submit a comment to a non-existant story.");
            }

            return result;
        }
    }
}
