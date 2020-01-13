using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Utf8Json;

namespace Zaabee.AspNetCore.Formatters.Utf8Json
{
    public class Utf8JsonInputFormatter : TextInputFormatter
    {
        public Utf8JsonInputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
            Encoding encoding)
        {
            var request = context.HttpContext.Request;
            return InputFormatterResult.SuccessAsync(request.Body.Unpack(context.ModelType));
        }
    }
}