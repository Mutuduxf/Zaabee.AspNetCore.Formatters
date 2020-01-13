using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Utf8Json;

namespace Zaabee.AspNetCore.Formatters.Utf8Json
{
    public class Utf8JsonOutputFormatter : TextOutputFormatter
    {
        public Utf8JsonOutputFormatter(MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            return response.WriteAsync(context.Object.ToJson());
        }
    }
}