using Pixora.DAL.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Storage
{
    public class SupabaseStorageAdapter : IImageStorageService
    {
        private readonly SupabaseStorageClient _client;
        public SupabaseStorageAdapter(SupabaseStorageClient client)
        {
            _client = client;
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) ||
                imageUrl.Contains("neutral_profile.png"))
            {
                return;
            }

            var basePublicUrl = $"{SupabaseConfig.PublicBaseUrl}/";
            var fileName = imageUrl.Replace(basePublicUrl, "");

            await _client.DeleteAsync(fileName);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileExtension, string contentType)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                return SupabaseConfig.DefaultImagePath;
            }

            var cleanExtension = fileExtension.StartsWith(".") ? fileExtension : $".{fileExtension}";
            var fileName = $"{Guid.NewGuid()}{cleanExtension}";

            return await _client.UploadAsync(imageStream, fileName, contentType);
        }
    }
}
