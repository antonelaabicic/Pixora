using Microsoft.AspNetCore.Identity;
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

        public AuthFacade(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false,  lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return AuthResult.Failure("Invalid email or password.");
            }

            return AuthResult.Success("Login successful.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
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

            return AuthResult.Success("Registration successful.");
        }
    }
}
