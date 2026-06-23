using Pixora.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Users
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto? GetById(string id);
        void UpdateUser(UpdateUserDto dto);
    }
}
