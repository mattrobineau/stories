using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models;
using Stories.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IPasswordService PasswordService;

        public AuthenticationService(IDbContext storiesDbContext, IPasswordService passwordService)
        {
            StoriesDbContext = storiesDbContext;
            PasswordService = passwordService;
        }

        public async Task<UserModel> AuthenticateUser(string email, string password)
        {
            var user = await StoriesDbContext.Users.Where(u => u.Email == email).Include(u => u.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

            if (user == null)
                return null;

            return AuthenticateUser(user, password);
        }

        public async Task<UserModel> AutenticateUser(Guid userId, string password)
        {
            var user = await StoriesDbContext.Users.Where(u => u.Id.Equals(userId)).Include(u => u.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

            return AuthenticateUser(user, password);
        }

        private UserModel AuthenticateUser(User user, string password)
        {
            if (!PasswordService.VerifyUserPassword(password, user?.PasswordHash, user?.PasswordSalt))
                return null;

            var roles = user.Roles.Select(r => new RoleModel
            {
                Id = r.RoleId,
                Name = r.Role.Name
            }).ToList();

            return new UserModel
            {
                Email = user.Email,
                Roles = roles,
                UserId = user.Id,
                Username = user.Username
            };
        }

        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var user = await StoriesDbContext.Users.Where(u => u.Id.Equals(model.UserId)).Include(u => u.Roles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

            if (user == null)
                return false;

            var passwordModel = PasswordService.HashPassword(model.NewPassword);

            user.PasswordHash = passwordModel.Hash;
            user.PasswordSalt = passwordModel.Salt;

            return await StoriesDbContext.SaveChangesAsync() == 1;            
        }
    }
}
