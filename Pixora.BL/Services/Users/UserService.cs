using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _userRepository
                .GetAll()
                .Select(u => _mapper.Map<UserDto>(u));
        }

        public UserDto? GetById(string id)
        {
            var user = _userRepository.GetById(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public void UpdateUser(UpdateUserDto dto)
        {
            var user = _userRepository.GetById(dto.UserId) ?? throw new InvalidOperationException("User not found.");

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.NormalizedEmail = dto.Email.ToUpper();
                user.NormalizedUserName = dto.Email.ToUpper();
            }

            if (dto.PlanType.HasValue)
            {
                user.PlanType = dto.PlanType.Value;
            }

            _userRepository.Update(user);
            _userRepository.Save();
        }
    }
}
