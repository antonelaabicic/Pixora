using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Plans;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Auth
{
    public class AuthFacade : IAuthFacade
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IUserActionLogService _logService;
        private readonly IPlanService _planService;
        public AuthFacade(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService, IUserActionLogService logService, IPlanService planService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _logService = logService;
            _planService = planService;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return AuthResult.Failure("Invalid email or password.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AuthResult.Failure("Invalid email or password.");
            }

            _planService.ApplyPendingPlanIfNeeded(user.Id);

            _logService.Log(user.Id, UserActionType.LoggedIn, $"{email} logged in.");

            var token = await _jwtService.GenerateTokenAsync(user);

            return AuthResult.Success("Login successful.", token);
        }

        public async Task LogoutAsync(string? userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                _logService.Log(userId, UserActionType.LoggedOut, $"{user?.Email} logged out.");
            }
        }

        public async Task<AuthResult> RegisterAsync(string email, string password, PlanType planType)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return AuthResult.Failure("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PlanType = planType,
                StorageUsedBytes = 0
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(e => e.Description));
                return AuthResult.Failure(message);
            }

            await _userManager.AddToRoleAsync(user, "User");

            _logService.Log(user.Id, UserActionType.Registered, $"{email} registered with {planType} plan.");

            var token = await _jwtService.GenerateTokenAsync(user);

            return AuthResult.Success("Registration successful.", token);
        }

        public AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl, PlanType planType)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["planType"] = planType.ToString();

            return properties;
        }

        public async Task<AuthResult> ExternalLoginCallbackAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return AuthResult.Failure("External login information not found.");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,                 info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                var existingUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (existingUser == null)
                {
                    return AuthResult.Failure("External user not found.");
                }
                _logService.Log(existingUser.Id, UserActionType.LoggedIn, $"{existingUser.Email} logged in with {info.LoginProvider}.");

                var existingToken = await _jwtService.GenerateTokenAsync(existingUser);
                return AuthResult.Success("External login successful.", existingToken);
            }

            var email = info.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                return AuthResult.Failure("Email was not provided by external provider.");
            }

            var planType = PlanType.Free;

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                PlanType = planType,
                StorageUsedBytes = 0
            };

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                var message = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return AuthResult.Failure(message);
            }

            await _userManager.AddLoginAsync(user, info);
            await _userManager.AddToRoleAsync(user, "User");

            _logService.Log(user.Id, UserActionType.Registered, $"{user.Email} registered with {info.LoginProvider}.");
            _logService.Log(user.Id, UserActionType.LoggedIn, $"{user.Email} logged in with {info.LoginProvider}.");

            var token = await _jwtService.GenerateTokenAsync(user);

            return AuthResult.Success("External registration successful.", token);
        }
    }
}
