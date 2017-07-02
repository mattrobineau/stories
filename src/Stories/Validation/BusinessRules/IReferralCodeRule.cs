namespace Stories.Validation.BusinessRules
{
    public interface IReferralCodeRule
    {
        ValidationResult Validate(string code);
    }
}
