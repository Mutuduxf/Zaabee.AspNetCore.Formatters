using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilOutputFormatter : OutputFormatter
    {
        private readonly Options _jilOptions;

        public JilOutputFormatter(string contentType, Options jilOptions, MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            _jilOptions = jilOptions;
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(contentType));
            SupportedMediaTypes.Add(mediaTypeHeaderValue);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            using (var writer = context.WriterFactory(response.Body, Encoding.UTF8))
            {
                JSON.Serialize(context.Object, writer, _jilOptions);
                return writer.FlushAsync();
            }
        }
    }
}