using Stories.Models.StoryViewModels;
using System;

namespace Stories.Validation.Validators
{
    public class CreateViewModelValidator : IValidator<CreateViewModel>
    {
        public ValidationResult Validate(CreateViewModel instance)
        {
            var result = new ValidationResult { IsValid = true };

            if(string.IsNullOrEmpty(instance.Title))
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
