using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public class ProPlanPolicy : PlanPolicyBase
    {
        public override PlanType PlanType => PlanType.Pro;

        public override long MaxFileSizeBytes => 10 * 1024 * 1024;

        public override int MaxDailyUploads => 20;

        public override long MaxStorageBytes => 500 * 1024 * 1024;

        public override bool AllowsAdvancedFilters => true;
    }
}
