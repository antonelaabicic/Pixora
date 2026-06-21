using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Plans
{
    public abstract class PlanPolicyBase : IPlanPolicy
    {
        public abstract PlanType PlanType { get; }

        public abstract long MaxFileSizeBytes { get; }

        public abstract int MaxDailyUploads { get; }

        public abstract long MaxStorageBytes { get; }

        public abstract bool AllowsAdvancedFilters { get; }

        public virtual bool CanUpload(long fileSizeBytes, int uploadsToday, long currentStorageUsedBytes)
        {
            return fileSizeBytes <= MaxFileSizeBytes && uploadsToday < MaxDailyUploads
                && currentStorageUsedBytes + fileSizeBytes <= MaxStorageBytes;
        }
    }
}
