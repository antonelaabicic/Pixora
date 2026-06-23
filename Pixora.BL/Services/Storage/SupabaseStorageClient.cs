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
        public async Task<string> UploadAsync(Stream imageStream, string fileName, string contentType)
        {
            var storageUrl = ConfigManager.SupabaseStorageObjectUrl(fileName);

            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ConfigManager.SupabaseApiKey);

            using var content = new StreamContent(imageStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await httpClient.PutAsync(storageUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Supabase upload failed: {error}");
            }

            return ConfigManager.SupabasePublicUrl(fileName);
        }
        public async Task DeleteAsync(string fileName)
        {
            var storageUrl = ConfigManager.SupabaseStorageObjectUrl(fileName);

            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ConfigManager.SupabaseApiKey);

            var response = await httpClient.DeleteAsync(storageUrl);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Supabase delete failed: {error}");
            }
        }
    }
}
