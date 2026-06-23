using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class EditPhotoMetadataDto
    {
        public int PhotoId { get; set; }

        public string UserId { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<string> Hashtags { get; set; } = new List<string>();
    }
}
