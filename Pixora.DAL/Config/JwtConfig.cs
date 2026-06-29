namespace Pixora.DAL.Config
{
    public static class JwtConfig
    {
        public static string JwtSecret => EnvironmentConfig.GetRequiredEnv("JWT_SECRET", 32);
        public static string JwtIssuer => EnvironmentConfig.GetRequiredEnv("JWT_ISSUER");
        public static string JwtAudience => EnvironmentConfig.GetRequiredEnv("JWT_AUDIENCE");
    }
}
