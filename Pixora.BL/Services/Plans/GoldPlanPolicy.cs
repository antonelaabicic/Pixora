using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public class GoldPlanPolicy : PlanPolicyBase
    {
        public override PlanType PlanType => PlanType.Gold;

        protected override long MaxFileSizeBytes => 50 * 1024 * 1024;

        protected override int MaxDailyUploads => 50;

        protected override long MaxStorageBytes => 2L * 1024 * 1024 * 1024;
    }
}
