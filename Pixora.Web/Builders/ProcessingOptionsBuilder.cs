

using Pixora.BL.Models;

namespace Pixora.Web.Builders
{
    public class ProcessingOptionsBuilder
    {
        private readonly ProcessingOptions _options = new();

        public ProcessingOptionsBuilder WithResize(bool resize, int? width, int? height)
        {
            _options.Resize = resize;
            _options.Width = resize ? width : null;
            _options.Height = resize ? height : null;

            return this;
        }

        public ProcessingOptionsBuilder WithSepia(bool applySepia)
        {
            _options.ApplySepia = applySepia;
            return this;
        }

        public ProcessingOptionsBuilder WithBlur(bool applyBlur, float blurAmount)
        {
            _options.ApplyBlur = applyBlur;
            _options.BlurAmount = applyBlur ? blurAmount : 0;

            return this;
        }

        public ProcessingOptionsBuilder WithFormat(string outputFormat)
        {
            _options.OutputFormat = string.IsNullOrWhiteSpace(outputFormat)
                ? "jpg"
                : outputFormat.Trim().ToLower();

            return this;
        }

        public ProcessingOptions Build()
        {
            return _options;
        }
    }
}