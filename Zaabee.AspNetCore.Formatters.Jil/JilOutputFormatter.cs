using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Jil;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilOutputFormatter : TextOutputFormatter
    {
        private readonly Options _jilOptions;

        public JilOutputFormatter(Options jilOptions, MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            _jilOptions = jilOptions;
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            return response.WriteAsync(context.Object.ToJson(_jilOptions));
        }
    }
}