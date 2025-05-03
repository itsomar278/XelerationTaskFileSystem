using System.Security.Cryptography;
using XelerationTask.Core.Interfaces;

namespace XelerationTask.Infastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public (byte[] passwordHash, byte[] passwordSalt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }

        public bool VerifyHashedPassword(byte[] passwordHash, byte[] passwordSalt, string providedPassword)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(providedPassword));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
