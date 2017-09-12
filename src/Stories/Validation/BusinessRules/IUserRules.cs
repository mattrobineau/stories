using System;

namespace Stories.Validation.BusinessRules
{
    public interface IUserRules
    {
        bool UserExists(Guid userId);        
    }
}
