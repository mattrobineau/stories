using Microsoft.EntityFrameworkCore;
using Stories.Constants;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models;
using Stories.Models.Administration;
using Stories.Models.User;
using Stories.Models.ViewModels.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContext StoriesDbContext;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IPasswordService PasswordService;

        public UserService(IDbContext storiesDbContext, IAuthenticationService authenticationService, IPasswordService passwordService)
        {
            PasswordService = passwordService;
            StoriesDbContext = storiesDbContext;
            AuthenticationService = authenticationService;
        }

        public async Task<UsersViewModel> GetUsers(int page, int count)
        {
            var users = await StoriesDbContext.Users.Include(u => u.Roles)
                                                    .ThenInclude(r => r.Role)
                                                    .Skip(page * count)
                                                    .Take(count)
                                                    .Select(u => u)
                                                    .ToListAsync();

            var userModels = users.Select(u => new UserSummaryViewModel
            { // https://github.com/aspnet/EntityFramework/issues/7714
                Username = u.Username,
                UserId = u.Id,
                CreatedDate = u.CreatedDate,
                IsBanned = u.IsBanned,
                IsModerator = u.Roles.Any(r => r.Role.Name == Roles.Admin || r.Role.Name == Roles.Moderator)
            }).ToList();

            return new UsersViewModel { Users = userModels };
        }

        public async Task<UserModel> CreateUser(CreateUserModel model)
        {
            var user = await StoriesDbContext.Users.Where(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)
                                                                    || u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
            // TODO: validation
            if (user != null)
            {
                return null;
            }

            var passwordModel = PasswordService.HashPassword(model.Password);

            var newUser = await StoriesDbContext.Users.AddAsync(new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordModel.Hash,
                PasswordSalt = passwordModel.Salt
            });

            var userRoles = new List<UserRole>();
            var roles = await StoriesDbContext.Roles.ToListAsync();

            if(!model.Roles.Any())
            {
                userRoles.Add(new UserRole { User = newUser.Entity, RoleId = roles.FirstOrDefault(r => r.Name == Roles.User).Id });
            }

            foreach (var role in model.Roles)
            {
                userRoles.Add(new UserRole { User = newUser.Entity, RoleId = roles.FirstOrDefault(r => r.Name == role).Id });
            }

            await StoriesDbContext.UserRoles.AddRangeAsync(userRoles);

            int rowcount = await StoriesDbContext.SaveChangesAsync();

            var userModel = new UserModel()
            {
                Email = newUser.Entity.Email,
                Username = newUser.Entity.Username,
                UserId = newUser.Entity.Id,
                Roles = newUser.Entity.Roles.Select(r => new RoleModel { Id = r.RoleId, Name = r.Role.Name }).ToList(),
                CreatedDate = newUser.Entity.CreatedDate
            };

            return userModel;
        }

        public bool DeleteUser()
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetUser(Guid userId)
        {
            var user = await StoriesDbContext.Users.Where(u => u.Id.Equals(userId))
                                                   .Include(u => u.Roles)
                                                   .ThenInclude(ur => ur.Role)
                                                   .SingleAsync();

            return new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                IsBanned = user.IsBanned,
                CreatedDate = user.CreatedDate,
                Roles = user.Roles.Select(r => new RoleModel { Id = r.RoleId, Name = r.Role.Name }).ToList(),
                UserId = user.Id
            };
        }

        public bool UpdateUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BanUser(BanUserModel model)
        {
            var userBan = await StoriesDbContext.UserBans.AddAsync(new UserBan {
                BannedByUserId = model.BannedByUserId,
                ExpiryDate = model.ExpiryDate,
                Notes = model.Notes,
                Reason = model.Reason,
                UserId = model.UserId
            });

            return await StoriesDbContext.SaveChangesAsync() > 1;
        }

        public async Task<bool> UsernameAvailable(string username)
        {
            return await StoriesDbContext.Users.Where(u => u.Username == username).AnyAsync();
        }
    }
}
