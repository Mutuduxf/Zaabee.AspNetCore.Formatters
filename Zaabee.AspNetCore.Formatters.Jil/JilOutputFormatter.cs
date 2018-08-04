using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilOutputFormatter : OutputFormatter
    {
        public JilOutputFormatter(string contentType)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(contentType));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            using (var reader = context.WriterFactory(response.Body, Encoding.UTF8))
                return Task.FromResult(JSON.Serialize(reader));
        }
    }
}