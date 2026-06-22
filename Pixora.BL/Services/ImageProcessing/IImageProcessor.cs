using Pixora.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.Services.ImageProcessing
{
    public interface IImageProcessor
    {
        Stream Process(Stream inputStream, ProcessingOptions options);
    }
}
