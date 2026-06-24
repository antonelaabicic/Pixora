namespace Pixora.Web.Services
{
    public class AuthStateService
    {
        public bool IsLoggedIn { get; private set; }
        public string? Role { get; private set; }

        public event Action? OnChange;

        public void SetUser(string? role)
        {
            Role = role;
            IsLoggedIn = !string.IsNullOrWhiteSpace(role);
            NotifyStateChanged();
        }

        public void Logout()
        {
            Role = null;
            IsLoggedIn = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
