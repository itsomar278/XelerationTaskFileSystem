using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Interfaces;

namespace XelerationTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO dto) => Ok(await _authService.RegisterAsync(dto));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto) => Ok(await _authService.LoginAsync(dto));

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshDTO dto) => Ok(await _authService.RefreshTokenAsync(dto));

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() => await _authService.LogoutAsync(User).ContinueWith(_ => Ok("Successfully logged out"));
    }
}
