using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public class ResizeStep : ImageProcessingStepBase
    {
        protected override void Apply(ImageProcessingRequest request)
        {
            if (!request.Options.Resize || !request.Options.Width.HasValue || !request.Options.Height.HasValue) {  
                return; 
            }
            request.Image.Mutate(x => x.Resize(request.Options.Width.Value, request.Options.Height.Value));
        }
    }
}
