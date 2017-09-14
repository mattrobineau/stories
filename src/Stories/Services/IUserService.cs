using Stories.Models.Users;
using Stories.Models.ViewModels.Administration;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUser(Guid userId);
        Task<UserModel> GetUser(string username);
        Task<UsersViewModel> GetUsers(int page, int count);
        Task<UserModel> CreateUser(CreateUserModel model);
        bool DeleteUser();
        bool UpdateUser();
        Task<bool> UsernameAvailable(string username);
    }
}
