using Pixora.BL.Services.ImageProcessing.Steps;

namespace Pixora.BL.Services.ImageProcessing
{
    public class ImageProcessingPipelineFactory : IImageProcessingPipelineFactory
    {
        public IImageProcessingStep CreatePipeline()
        {
            var resizeStep = new ResizeStep();
            var sepiaStep = new SepiaStep();
            var blurStep = new BlurStep();
            var formatStep = new FormatStep();

            resizeStep.SetNext(sepiaStep);
            sepiaStep.SetNext(blurStep);
            blurStep.SetNext(formatStep);

            return resizeStep;
        }
    }
}
