using Pixora.BL.DTOs;
using Pixora.BL.Models;
using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Photos
{
    public interface IPhotoService
    {
        Task<int> UploadPhotoAsync(UploadPhotoDto dto);

        IEnumerable<Photo> GetAll();

        IEnumerable<Photo> GetLatest(int count);

        Photo? GetById(int id);

        IEnumerable<Photo> GetByAuthorId(string authorId);

        IEnumerable<Photo> Search(PhotoSearchDto dto);

        void EditMetadata(EditPhotoMetadataDto dto);

        Task DeleteAsync(int photoId, string userId, bool isAdmin = false);

        Task<Stream> DownloadOriginalAsync(int photoId);

        Task<Stream> DownloadProcessedAsync(DownloadProcessedPhotoDto dto);
    }
}
