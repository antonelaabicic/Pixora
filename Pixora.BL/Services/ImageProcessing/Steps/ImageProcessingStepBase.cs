using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public abstract class ImageProcessingStepBase : IImageProcessingStep
    {
        private IImageProcessingStep? _next;
        public void SetNext(IImageProcessingStep next)
        {
            _next = next;
        }
        public void Process(ImageProcessingRequest request)
        {
            Apply(request);
            _next?.Process(request);
        }
        protected abstract void Apply(ImageProcessingRequest request);
    }
}
