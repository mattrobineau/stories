using Stories.Models;

namespace Stories.Services
{
    public interface IPasswordService
    {
        PasswordModel HashPassword(string password);
        bool VerifyUserPassword(string password, string hashedPassword, string salt);
    }
}
