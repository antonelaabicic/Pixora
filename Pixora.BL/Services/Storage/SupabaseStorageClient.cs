using Pixora.DAL.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.Storage
{
    public class SupabaseStorageClient
    {
        private readonly HttpClient _client;
        public SupabaseStorageClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Supabase");
        }

        public async Task<string> UploadAsync(Stream imageStream, string fileName, string contentType)
        {
            var storageUrl = SupabaseConfig.StorageObjectUrl(fileName);

            using var content = new StreamContent(imageStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await _client.PutAsync(storageUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Supabase upload failed: {error}");
            }

            return SupabaseConfig.PublicUrl(fileName);
        }
        public async Task DeleteAsync(string fileName)
        {
            var storageUrl = SupabaseConfig.StorageObjectUrl(fileName);

            var response = await _client.DeleteAsync(storageUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Supabase delete failed: {error}");
            }
        }
    }
}
