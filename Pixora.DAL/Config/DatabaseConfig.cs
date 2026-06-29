namespace Pixora.DAL.Config
{
    public static class DatabaseConfig
    {
        public static string ConnectionString => EnvironmentConfig.GetRequiredEnv("PSQL_CONNECTION_STRING");
    }
}
