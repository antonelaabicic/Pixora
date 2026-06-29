namespace Pixora.DAL.Config
{
    public static class GithubConfig
    {
        public static string ClientId => EnvironmentConfig.GetRequiredEnv("GITHUB_CLIENT_ID");
        public static string ClientSecret => EnvironmentConfig.GetRequiredEnv("GITHUB_CLIENT_SECRET");
    }
}
