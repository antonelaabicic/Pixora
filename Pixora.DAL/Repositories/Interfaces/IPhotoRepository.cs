using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        IEnumerable<Photo> GetLatest(int count);
        IEnumerable<Photo> GetByAuthorId(string authorId);
        IEnumerable<Photo> Search(string? hashtag, long? minSizeBytes, long? maxSizeBytes, DateTime? uploadedFrom,
            DateTime? uploadedTo, string? authorEmail);
    }
}
