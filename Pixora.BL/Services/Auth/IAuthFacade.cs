using Microsoft.AspNetCore.Authentication;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Auth
{
    public interface IAuthFacade
    {
        Task<AuthResult> RegisterAsync(string email, string password, PlanType planType);
        Task<AuthResult> LoginAsync(string email, string password);
        Task LogoutAsync(string? userId);

        AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl, PlanType planType);
        Task<AuthResult> ExternalLoginCallbackAsync();
    }
}
