using Pixora.BL.DTOs;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Logs
{
    public interface IUserActionLogService
    {
        void Log(string? userId, UserActionType actionType, string details = "");
        IEnumerable<UserActionLogDto> GetAll();
        IEnumerable<UserActionLogDto> GetByUserId(string userId);
        IEnumerable<UserActionLogDto> GetLatest(int count);
    }
}
