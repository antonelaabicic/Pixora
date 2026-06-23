using Microsoft.EntityFrameworkCore;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Impl
{
    public class UserActionLogRepository : IUserActionLogRepository
    {
        private readonly PixoraContext _context;
        public UserActionLogRepository(PixoraContext context)
        {
            _context = context;
        }

        public IEnumerable<UserActionLog> GetAll()
        {
            return _context.UserActionLogs
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public UserActionLog? GetById(int id)
        {
            return _context.UserActionLogs
                .Include(l => l.User)
                .FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<UserActionLog> GetByUserId(string userId)
        {
            return _context.UserActionLogs
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }

        public IEnumerable<UserActionLog> GetLatest(int count)
        {
            return _context.UserActionLogs
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToList();
        }

        public void Insert(UserActionLog log)
        {
            _context.UserActionLogs.Add(log);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
