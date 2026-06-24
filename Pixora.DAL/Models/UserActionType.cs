using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Models
{
    public enum UserActionType
    {
        Registered,
        LoggedIn,
        LoggedOut,
        UploadedPhoto,
        EditedPhoto,
        DeletedPhoto,
        DownloadedPhoto,
        ChangedPlan,
        AdminUpdatedUser,
        AdminDeletedPhoto
    }
}
