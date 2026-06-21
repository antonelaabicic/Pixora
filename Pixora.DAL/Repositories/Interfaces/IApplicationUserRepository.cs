using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser? GetById(string id);
        ApplicationUser? GetByEmail(string email);
        IEnumerable<ApplicationUser> GetByPlanType(PlanType planType);
        void Update(ApplicationUser user);
        void Save();
    }
}
