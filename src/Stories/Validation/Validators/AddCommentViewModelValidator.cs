﻿using Stories.Models.ViewModels;
using Stories.Services;
using System.Threading.Tasks;

namespace Stories.Validation.Validators
{
    public class AddCommentViewModelValidator : IValidator<AddCommentViewModel>
    {
        private readonly IUserService UserService;

        public AddCommentViewModelValidator(IUserService userService)
        {
            UserService = userService;
        }

        public ValidationResult Validate(AddCommentViewModel instance)
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

            if (string.IsNullOrEmpty(instance.CommentMarkdown))
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
