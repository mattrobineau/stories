using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Stories.Models;
using Stories.Models.User;
using Stories.Models.ViewModels.User;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordModel HashPassword(string password)
        {
            return HashPassword(password, null);
        }

        public bool VerifyUserPassword(string password, string hashedPassword, string salt)
        {
            var model = HashPassword(password, Convert.FromBase64String(salt));
            return model.Hash.Equals(hashedPassword);
        }

        private PasswordModel HashPassword(string password, byte[] salt = null)
        {
            var model = new PasswordModel();

            // generate a 128-bit salt using a secure PRNG
            if (salt == null)
            {
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            model.Salt = Convert.ToBase64String(salt);

            model.Hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                // TODO Use argon2 when available
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return model;
        }
    }
}
