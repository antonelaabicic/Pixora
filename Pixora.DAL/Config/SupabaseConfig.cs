namespace Pixora.DAL.Config
{
    public static class SupabaseConfig
    {
        public static string Url => EnvironmentConfig.GetRequiredEnv("SUPABASE_URL");
        public static string Bucket => EnvironmentConfig.GetRequiredEnv("SUPABASE_BUCKET");
        public static string ApiKey => EnvironmentConfig.GetRequiredEnv("SUPABASE_API_KEY");

        public static string PublicBaseUrl => $"{Url}/storage/v1/object/public/{Bucket}";
        public static string DefaultImagePath => $"{PublicBaseUrl}/neutral_profile.png";

        public static string StorageObjectUrl(string fileName) => $"{Url}/storage/v1/object/{Bucket}/{fileName}";
        public static string PublicUrl(string fileName) => $"{PublicBaseUrl}/{fileName}";
    }
}
