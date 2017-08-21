namespace Stories.Validation.Validators
{
    public class NullValidator<T> : IValidator<T>
    {
        public ValidationResult Validate(T instance)
        {
            return new ValidationResult { IsValid = true };
        }
    }
}
