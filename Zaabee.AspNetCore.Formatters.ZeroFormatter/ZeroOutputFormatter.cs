using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.ZeroFormatter;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public class ZeroOutputFormatter : OutputFormatter
    {
        public ZeroOutputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.Body.PackBy(context.ObjectType, context.Object);
            return Task.CompletedTask;
        }
    }
}