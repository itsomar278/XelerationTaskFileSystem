using System.Security.Claims;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;

namespace XelerationTask.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserCreateDTO userCreateDTO);

        Task<UserLoginResultDTO> LoginAsync(UserLoginDTO userLoginDTO);

        Task<UserLoginResultDTO> RefreshTokenAsync(TokenRefreshDTO tokenRefreshDTO);

        Task LogoutAsync(ClaimsPrincipal user);
    }
}
