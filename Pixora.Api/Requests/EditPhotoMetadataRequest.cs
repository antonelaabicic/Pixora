namespace Pixora.Api.Requests
{
    public class EditPhotoMetadataRequest
    {
        public string Description { get; set; } = string.Empty;

        public List<string> Hashtags { get; set; } = new();
    }
}
