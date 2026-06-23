using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public class PlanService : IPlanService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PlanService(IApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public void ApplyPendingPlanIfNeeded(string userId)
        {
            var user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (user.PendingPlanType.HasValue && user.PendingPlanActiveFrom.HasValue &&             user.PendingPlanActiveFrom.Value <= today)
            {
                user.PlanType = user.PendingPlanType.Value;
                user.PendingPlanType = null;
                user.PendingPlanActiveFrom = null;

                _userRepository.Update(user);
                _userRepository.Save();
            }
        }

        public UserDto GetUserPlan(string userId)
        {
            var user = _userRepository.GetById(userId) ?? throw new InvalidOperationException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public void RequestPlanChange(ChangePlanDto dto)
        {
            var user = _userRepository.GetById(dto.UserId) ?? throw new InvalidOperationException("User not found.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (user.LastPlanChangeDate == today)
            {
                throw new InvalidOperationException("Plan can only be changed once per day.");
            }

            user.PendingPlanType = dto.NewPlanType;
            user.PendingPlanActiveFrom = today.AddDays(1);
            user.LastPlanChangeDate = today;

            _userRepository.Update(user);
            _userRepository.Save();
        }
    }
}
