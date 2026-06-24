using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class DownloadPhotoDto
    {
        public Stream Stream { get; set; } = null!;

        public string FileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;
    }
}
