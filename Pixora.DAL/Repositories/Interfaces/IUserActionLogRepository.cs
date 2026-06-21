using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IUserActionLogRepository : IRepository<UserActionLog>
    {
        IEnumerable<UserActionLog> GetByUserId(string userId);
        IEnumerable<UserActionLog> GetLatest(int count);
    }
}
