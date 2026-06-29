namespace Pixora.DAL.Config
{
    public static class GoogleConfig
    {
        public static string ClientId => EnvironmentConfig.GetRequiredEnv("GOOGLE_CLIENT_ID");
        public static string ClientSecret => EnvironmentConfig.GetRequiredEnv("GOOGLE_CLIENT_SECRET");
    }
}
