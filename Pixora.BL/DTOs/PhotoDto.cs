using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string AuthorId { get; set; } = null!;
        public string? AuthorEmail { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = null!;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; }
        public List<string> Hashtags { get; set; } = new();
    }
}
