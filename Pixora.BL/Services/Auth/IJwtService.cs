using Pixora.BL.DTOs;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Auth
{
    public interface IJwtService
    {
        Task<AuthResponseDto> GenerateTokenAsync(ApplicationUser user);
    }
}
