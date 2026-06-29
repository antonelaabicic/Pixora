using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Models
{
    public class ProcessingOptions
    {
        public bool Resize { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public bool ApplySepia { get; set; }
        public bool ApplyBlur { get; set; }
        public float BlurAmount { get; set; } = 3f;
        public bool ApplyGrayscale { get; set; }

        public string OutputFormat { get; set; } = "jpg";
    }
}
