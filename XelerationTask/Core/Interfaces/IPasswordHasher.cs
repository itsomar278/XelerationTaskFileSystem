namespace XelerationTask.Core.Interfaces
{
    public interface IPasswordHasher
    {
        (byte[] passwordHash, byte[] passwordSalt) HashPassword(string rawPassword);
        bool VerifyHashedPassword(byte[] passwordHash, byte[] passwordSalt, string providedPassword);
    }
}
