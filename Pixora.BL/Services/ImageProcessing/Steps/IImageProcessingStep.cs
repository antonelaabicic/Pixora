using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public interface IImageProcessingStep
    {
        void SetNext(IImageProcessingStep next);
        void Process(ImageProcessingRequest request);
    }
}
