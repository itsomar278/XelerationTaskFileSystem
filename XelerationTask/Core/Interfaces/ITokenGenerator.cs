using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface ITokenGenerator
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();

    }
}
