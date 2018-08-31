using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilInputFormatter : InputFormatter
    {
        private readonly Options _jilOptions;

        public JilInputFormatter(Options jilOptions, MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            _jilOptions = jilOptions;
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue.TryParse(request.ContentType, out _);

            using (var reader = context.ReaderFactory(request.Body, Encoding.UTF8))
                return InputFormatterResult.SuccessAsync(JSON.Deserialize(reader, context.ModelType, _jilOptions));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }
    }
}