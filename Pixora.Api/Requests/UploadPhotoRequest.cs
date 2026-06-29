using Pixora.BL.DTOs;
using Pixora.BL.Models;

namespace Pixora.Api.Requests
{
    public class UploadPhotoRequest
    {
        public IFormFile File { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public List<string> Hashtags { get; set; } = new();

        public bool Resize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public bool ApplySepia { get; set; }
        public bool ApplyBlur { get; set; }
        public float BlurAmount { get; set; } = 3f;

        public bool ApplyGrayscale { get; set; }

        public string OutputFormat { get; set; } = "jpg";

        public UploadPhotoDto ToDto(string userId)
        {
            return new UploadPhotoDto
            {
                UserId = userId,
                ImageStream = File.OpenReadStream(),
                OriginalFileName = File.FileName,
                ContentType = File.ContentType,
                FileSizeBytes = File.Length,
                Description = Description,
                Hashtags = Hashtags,
                ProcessingOptions = new ProcessingOptions
                {
                    Resize = Resize,
                    Width = Resize ? Width : null,
                    Height = Resize ? Height : null,
                    ApplySepia = ApplySepia,
                    ApplyBlur = ApplyBlur,
                    BlurAmount = BlurAmount,
                    ApplyGrayscale = ApplyGrayscale,
                    OutputFormat = OutputFormat
                }
            };
        }
    }
}