using Pixora.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public interface IPlanService
    {
        UserDto GetUserPlan(string userId);
        void RequestPlanChange(ChangePlanDto dto);
        void ApplyPendingPlanIfNeeded(string userId);
    }
}
