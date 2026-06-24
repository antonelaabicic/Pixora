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

        protected override long MaxFileSizeBytes => 100 * 1024;

        protected override int MaxDailyUploads => 2;

        protected override long MaxStorageBytes => 500 * 1024;
    }
}
