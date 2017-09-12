using Stories.Data.DbContexts;
using System;
using System.Linq;

namespace Stories.Validation.BusinessRules
{
    public class UserRules : IUserRules
    {
        private readonly IDbContext DbContext;

        public UserRules(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool UserExists(Guid userId)
        {
            return DbContext.Users.Any(u => u.Id == userId);
        }
    }
}
