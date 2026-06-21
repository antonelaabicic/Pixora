
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public PlanType PlanType { get; set; } = PlanType.Free;
        public PlanType? PendingPlanType { get; set; }
        public DateOnly? PendingPlanActiveFrom { get; set; }
        public DateOnly? LastPlanChangeDate { get; set; }
        public long StorageUsedBytes { get; set; }

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<UserActionLog> ActionLogs { get; set; } = new List<UserActionLog>();
    }
}
