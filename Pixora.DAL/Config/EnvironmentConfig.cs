using DotNetEnv;

namespace Pixora.DAL.Config
{
    public static class EnvironmentConfig
    {
        private static bool _loaded = false;
        private static void EnsureEnvLoaded()
        {
#if DEBUG
            if (_loaded) return;

            var envPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Pixora.DAL", "Resources", ".env"));
            if (!File.Exists(envPath))
                throw new FileNotFoundException($".env file not found at: {envPath}");

            Env.Load(envPath);
            _loaded = true;
#endif
        }

        public static string GetRequiredEnv(string key, int? requiredLength = null)
        {
            EnsureEnvLoaded();
            var value = Environment.GetEnvironmentVariable(key);

#if DEBUG
            if (string.IsNullOrWhiteSpace(value))
            {
                value = Env.GetString(key);
            }
#endif

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"{key} not found in environment.");
            }

            if (requiredLength.HasValue && value.Length != requiredLength.Value)
            {
                throw new InvalidOperationException($"{key} must be exactly {requiredLength.Value} characters.");
            }

            return value;
        }
    }
}
