using AutoMapper;
using Pixora.BL.DTOs;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Logs
{
    public class UserActionLogService : IUserActionLogService
    {
        private readonly IUserActionLogRepository _logRepository;
        private readonly IMapper _mapper;

        public UserActionLogService(IUserActionLogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserActionLogDto> GetAll()
        {
            return _logRepository.GetAll().Select(log => _mapper.Map<UserActionLogDto>(log));
        }

        public IEnumerable<UserActionLogDto> GetByUserId(string userId)
        {
            return _logRepository.GetByUserId(userId).Select(log => _mapper.Map<UserActionLogDto>(log));
        }

        public IEnumerable<UserActionLogDto> GetLatest(int count)
        {
            return _logRepository.GetLatest(count).Select(log => _mapper.Map<UserActionLogDto>(log));
        }

        public void Log(string? userId, UserActionType actionType, string details = "")
        {
            var log = new UserActionLog
            {
                UserId = userId,
                ActionType = actionType,
                Details = details,
                CreatedAt = DateTime.UtcNow
            };

            _logRepository.Insert(log);
            _logRepository.Save();
        }
    }
}
