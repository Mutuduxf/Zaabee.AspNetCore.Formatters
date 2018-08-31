using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilOutputFormatter : TextOutputFormatter
    {
        private readonly Options _jilOptions;

        public JilOutputFormatter(Options jilOptions, MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            _jilOptions = jilOptions;
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            return response.WriteAsync(JSON.Serialize(context.Object, _jilOptions));
        }

        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }
    }
}