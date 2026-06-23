using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public class FreePlanPolicy : PlanPolicyBase
    {
        public override PlanType PlanType => PlanType.Free;

        protected override long MaxFileSizeBytes => 2 * 1024 * 1024;

        protected override int MaxDailyUploads => 5;

        protected override long MaxStorageBytes => 50 * 1024 * 1024;
    }
}
