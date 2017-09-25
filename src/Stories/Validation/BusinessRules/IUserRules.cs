using System;

namespace Stories.Validation.BusinessRules
{
    public interface IUserRules
    {
        bool UserExists(Guid userId);
        bool ActionPreventedByUserBan(string email);
        bool ActionPreventedByUserBan(Guid userId);
    }
}
