using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Utf8Json;
using Zaabee.Utf8Json;

namespace Zaabee.AspNetCore.Formatters.Utf8Json
{
    public class Utf8JsonOutputFormatter : TextOutputFormatter
    {
        private readonly IJsonFormatterResolver _resolver;

        public Utf8JsonOutputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue,
            IJsonFormatterResolver resolver = null)
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
            _resolver = resolver;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            await response.WriteAsync(Utf8JsonSerializer.SerializeToJson(context.Object, _resolver));
        }
    }
}