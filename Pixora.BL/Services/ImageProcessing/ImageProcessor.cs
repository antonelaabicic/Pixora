using Pixora.BL.Models;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageProcessingPipelineFactory _pipelineFactory;
        public ImageProcessor(IImageProcessingPipelineFactory pipelineFactory)
        {
            _pipelineFactory = pipelineFactory;
        }
        public Stream Process(Stream inputStream, ProcessingOptions options)
        {
            using var image = Image.Load(inputStream);

            var request = new ImageProcessingRequest
            {
                Image = image,
                Options = options
            };

            var pipeline = _pipelineFactory.CreatePipeline();

            pipeline.Process(request);

            return request.OutputStream;
        }
    }
}
