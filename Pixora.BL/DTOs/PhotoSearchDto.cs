using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class PhotoSearchDto
    {
        public string? Hashtag { get; set; }

        public long? MinSizeBytes { get; set; }

        public long? MaxSizeBytes { get; set; }

        public DateTime? UploadedFrom { get; set; }

        public DateTime? UploadedTo { get; set; }

        public string? AuthorId { get; set; }
    }
}
