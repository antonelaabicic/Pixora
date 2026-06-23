using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public PlanType PlanType { get; set; }
        public PlanType? PendingPlanType { get; set; }
        public DateOnly? PendingPlanActiveFrom { get; set; }
        public DateOnly? LastPlanChangeDate { get; set; }
        public long StorageUsedBytes { get; set; }
    }
}
