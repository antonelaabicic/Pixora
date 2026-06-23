using Pixora.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class UploadPhotoDto
    {
        public string UserId { get; set; } = null!;

        public Stream ImageStream { get; set; } = null!;

        public string OriginalFileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long FileSizeBytes { get; set; }

        public string Description { get; set; } = string.Empty;

        public IEnumerable<string> Hashtags { get; set; } = new List<string>();

        public ProcessingOptions ProcessingOptions { get; set; } = new();
    }
}
