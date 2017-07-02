namespace Stories.Validation.Validators
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T instance);
    }
}
