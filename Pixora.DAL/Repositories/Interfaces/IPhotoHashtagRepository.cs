using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IPhotoHashtagRepository
    {
        IEnumerable<PhotoHashtag> GetByPhotoId(int photoId);
        IEnumerable<PhotoHashtag> GetByHashtagId(int hashtagId);
        void Insert(PhotoHashtag entity);
        void Delete(PhotoHashtag entity);
        void DeleteByPhotoId(int photoId);
        void Save();
    }
}
