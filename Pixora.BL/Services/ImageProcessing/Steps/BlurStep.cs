using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public class BlurStep : ImageProcessingStepBase
    {
        protected override void Apply(ImageProcessingRequest request)
        {
            if (!request.Options.ApplyBlur)
            {
                return;
            }
            request.Image.Mutate(x => x.GaussianBlur(request.Options.BlurAmount));
        }
    }
}
