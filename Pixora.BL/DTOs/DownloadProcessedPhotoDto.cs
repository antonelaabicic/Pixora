using Pixora.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class DownloadProcessedPhotoDto
    {
        public int PhotoId { get; set; }

        public ProcessingOptions ProcessingOptions { get; set; } = new();
    }
}
