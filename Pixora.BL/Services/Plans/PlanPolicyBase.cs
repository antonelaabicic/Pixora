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

        protected abstract long MaxFileSizeBytes { get; }

        protected abstract int MaxDailyUploads { get; }

        protected abstract long MaxStorageBytes { get; }

        public bool CanUpload(long fileSizeBytes, int uploadsToday, long currentStorageUsedBytes)
        {
            return ValidateFileSize(fileSizeBytes) && ValidateDailyUploads(uploadsToday) && ValidateStorage(fileSizeBytes, currentStorageUsedBytes);
        }
        protected virtual bool ValidateFileSize(long fileSizeBytes)
        {
            return fileSizeBytes <= MaxFileSizeBytes;
        }
        protected virtual bool ValidateDailyUploads(int uploadsToday)
        {
            return uploadsToday < MaxDailyUploads;
        }
        protected virtual bool ValidateStorage(long fileSizeBytes, long currentStorageUsedBytes)
        {
            return currentStorageUsedBytes + fileSizeBytes <= MaxStorageBytes;
        }
    }
}
