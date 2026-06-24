using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace Pixora.Web.Services
{
    public class JwtService
    {
        private readonly IJSRuntime _js;
        public JwtService(IJSRuntime js)
        {
            _js = js;
        }
        public async Task<string?> GetUserIdAsync()
                    => await GetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

        public async Task<string?> GetEmailAsync()
            => await GetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

        public async Task<string?> GetRoleAsync()
            => await GetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        public async Task<string?> GetProperty(string property)
        {
            var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "jwt");
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var payload = token.Split('.')[1];
            var json = JsonSerializer.Deserialize<JsonElement>(
                Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)))
            );

            return json.TryGetProperty(property, out var role) ? role.GetString() : null;
        }
        private string PadBase64(string input)
        {
            return input.PadRight(input.Length + (4 - input.Length % 4) % 4, '=');
        }
    }
}
