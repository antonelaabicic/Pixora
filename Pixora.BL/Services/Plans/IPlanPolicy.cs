using Pixora.DAL.Models;

namespace Pixora.BL.Services.Plans
{
    public interface IPlanPolicy
    {
        PlanType PlanType { get; }
        bool CanUpload(long fileSizeBytes, int uploadsToday, long currentStorageUsedBytes);
    }
}
