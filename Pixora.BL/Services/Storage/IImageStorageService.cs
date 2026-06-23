using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Storage
{
    public interface IImageStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileExtension, string contentType);
        Task DeleteImageAsync(string imageUrl);
    }
}
