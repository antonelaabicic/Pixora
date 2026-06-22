using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public class FormatStep : ImageProcessingStepBase
    {
        protected override void Apply(ImageProcessingRequest request)
        {
            request.OutputStream = new MemoryStream();

            switch (request.Options.OutputFormat.ToLower())
            {
                case "png":
                    request.Image.Save(request.OutputStream, new PngEncoder());
                    break;

                case "bmp":
                    request.Image.Save(request.OutputStream, new BmpEncoder());
                    break;

                case "jpg":
                case "jpeg":
                default:
                    request.Image.Save(request.OutputStream, new JpegEncoder());
                    break;
            }

            request.OutputStream.Position = 0;
        }
    }
}
