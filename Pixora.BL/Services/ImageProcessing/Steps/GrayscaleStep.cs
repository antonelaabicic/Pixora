using SixLabors.ImageSharp.Processing;

namespace Pixora.BL.Services.ImageProcessing.Steps
{
    public class GrayscaleStep : ImageProcessingStepBase
    {
        protected override void Apply(ImageProcessingRequest request)
        {
			if (!request.Options.ApplyGrayscale)
			{
				return;
			}

			request.Image.Mutate(x => x.Grayscale());
		}
    }
}
