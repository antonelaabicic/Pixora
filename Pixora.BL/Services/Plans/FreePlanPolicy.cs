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

        public override long MaxFileSizeBytes => 2 * 1024 * 1024;

        public override int MaxDailyUploads => 5;

        public override long MaxStorageBytes => 50 * 1024 * 1024;

        public override bool AllowsAdvancedFilters => false;
    }
}
