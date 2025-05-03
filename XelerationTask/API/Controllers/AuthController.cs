using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using XelerationTask.Application.DTOs;
using XelerationTask.Core.Interfaces;
using XelerationTask.Core.Models;

namespace XelerationTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService , IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var mappedUser = _mapper.Map<User>(userCreateDTO);
            var result = await _authService.RegisterAsync(mappedUser, userCreateDTO.Password);
            var responseDTO = _mapper.Map<UserResponseDTO>(result);

            return Ok(responseDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.LoginAsync(userLoginDTO.Email, userLoginDTO.Passwrod);

            var resultDTO = _mapper.Map<UserLoginResultDTO>(result);

            return Ok(resultDTO);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshDTO tokenRefreshDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.RefreshTokenAsync(tokenRefreshDTO.RefreshToken);
            var resultDTO = _mapper.Map<UserLoginResultDTO>(result);
            return Ok(resultDTO);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
        
            await _authService.LogoutAsync(int.Parse(userId));
            return Ok("Successfuly logged out");
        }


    }

}
