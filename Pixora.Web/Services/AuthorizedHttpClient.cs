using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Pixora.Web.Services
{
    public class AuthorizedHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJSRuntime _js;

        public AuthorizedHttpClient(IHttpClientFactory httpClientFactory, IJSRuntime js)
        {
            _httpClientFactory = httpClientFactory;
            _js = js;
        }

        public async Task<HttpClient> GetClientAsync()
        {
            var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "jwt");
            var client = _httpClientFactory.CreateClient("Api");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
