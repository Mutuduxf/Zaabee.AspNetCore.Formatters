using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Utf8Json;
using Zaabee.Utf8Json;

namespace Zaabee.AspNetCore.Formatters.Utf8Json
{
    public class Utf8JsonInputFormatter : TextInputFormatter
    {
        private readonly IJsonFormatterResolver _resolver;

        public Utf8JsonInputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue, IJsonFormatterResolver resolver = null)
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
            _resolver = resolver;
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
            Encoding encoding)
        {
            var request = context.HttpContext.Request;
            return InputFormatterResult.SuccessAsync(Utf8JsonSerializer.Unpack(context.ModelType, request.Body,
                _resolver));
        }
    }
}