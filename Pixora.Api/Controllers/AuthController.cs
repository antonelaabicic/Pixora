using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixora.BL.DTOs;
using Pixora.BL.Services.Auth;
using Pixora.DAL.Models;
using System.Security.Claims;

namespace Pixora.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthFacade _authFacade;
        public AuthController(IAuthFacade authFacade)
        {
            _authFacade = authFacade;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authFacade.RegisterAsync(dto.Email, dto.Password, dto.PlanType);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authFacade.LoginAsync(dto.Email, dto.Password);

            if (!result.Succeeded)
            {
                return Unauthorized(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _authFacade.LogoutAsync(userId);

            return Ok("Logout successful.");
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin([FromQuery] PlanType planType = PlanType.Free)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth");
            var properties = _authFacade.ConfigureExternalLogin(GoogleDefaults.AuthenticationScheme,                redirectUrl!, planType);

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("github")]
        public IActionResult GitHubLogin([FromQuery] PlanType planType = PlanType.Free)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth");
            var properties = _authFacade.ConfigureExternalLogin(GitHubAuthenticationDefaults.AuthenticationScheme,
                redirectUrl!, planType);

            return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await _authFacade.ExternalLoginCallbackAsync();

            if (!result.Succeeded || result.Data == null)
            {
                return BadRequest(result.Message);
            }

            return Redirect($"https://localhost:7136/login-success?token={result.Data.Token}");
        }
    }
}