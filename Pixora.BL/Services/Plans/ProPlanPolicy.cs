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

        protected override long MaxFileSizeBytes => 10 * 1024 * 1024;

        protected override int MaxDailyUploads => 30;

        protected override long MaxStorageBytes => 500 * 1024 * 1024;
    }
}
