using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user , String rawPassword);

        Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password);

        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);

        Task LogoutAsync(int userId);
    }
}
