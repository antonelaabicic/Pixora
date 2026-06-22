using Pixora.BL.Services.ImageProcessing.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing
{
    public interface IImageProcessingPipelineFactory
    {
        IImageProcessingStep CreatePipeline();
    }
}
