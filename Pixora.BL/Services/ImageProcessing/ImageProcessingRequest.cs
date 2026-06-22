using Pixora.BL.Models;
using SixLabors.ImageSharp;

namespace Pixora.BL.Services.ImageProcessing
{
    public class ImageProcessingRequest
    {
        public Image Image { get; set; } = null!;

        public ProcessingOptions Options { get; set; } = null!;

        public MemoryStream OutputStream { get; set; } = new();
    }
}
