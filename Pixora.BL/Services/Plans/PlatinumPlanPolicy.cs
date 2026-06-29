using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public class PlatinumPlanPolicy : PlanPolicyBase
    {
        public override PlanType PlanType => PlanType.Platinum;

        protected override long MaxFileSizeBytes => 100 * 1024 * 1024;

        protected override int MaxDailyUploads => 100;

        protected override long MaxStorageBytes => 10L * 1024 * 1024 * 1024;
    }
}
