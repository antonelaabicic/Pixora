using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public class SepiaStep : ImageProcessingStepBase
    {
        protected override void Apply(ImageProcessingRequest request)
        {
            if (!request.Options.ApplySepia)
            {
                return;
            }
            request.Image.Mutate(x => x.Sepia());
        }
    }
}
