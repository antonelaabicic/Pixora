using Pixora.DAL.Models;

namespace Pixora.BL.Services.Plans
{
    public interface IPlanPolicy
    {
        PlanType PlanType { get; }

        long MaxFileSizeBytes { get; }

        int MaxDailyUploads { get; }

        long MaxStorageBytes { get; }

        bool AllowsAdvancedFilters { get; }

        bool CanUpload(long fileSizeBytes, int uploadsToday, long currentStorageUsedBytes);
    }
}
