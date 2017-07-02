using Stories.Services;
using System;

namespace Stories.Validation.BusinessRules
{
    public class ReferralCodeRule : IReferralCodeRule
    {
        public ValidationResult Validate(string code)
        {
            var model = new ValidationResult { IsValid = true };

            if (!Guid.TryParse(code, out Guid referralCode))
            {
                model.IsValid = false;
                model.Messages.Add("Referral code is not valid.");
            }
            
            return model;
        }
    }
}
