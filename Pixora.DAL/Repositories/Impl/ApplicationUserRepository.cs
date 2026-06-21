using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Impl
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly PixoraContext _context;
        public ApplicationUserRepository(PixoraContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users.OrderBy(u => u.Email).ToList();
        }

        public ApplicationUser? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.NormalizedEmail == email.ToUpper());
        }

        public ApplicationUser? GetById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<ApplicationUser> GetByPlanType(PlanType planType)
        {
            return _context.Users.Where(u => u.PlanType == planType).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ApplicationUser user)
        {
            _context.Users.Update(user);
        }
    }
}
