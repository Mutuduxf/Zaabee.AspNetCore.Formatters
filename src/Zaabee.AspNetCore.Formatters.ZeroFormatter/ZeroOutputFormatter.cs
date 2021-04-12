using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Extensions;
using Zaabee.ZeroFormatter;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public class ZeroOutputFormatter : OutputFormatter
    {
        public ZeroOutputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            await ZeroSerializer.Serialize(context.ObjectType, context.Object)
                .WriteToAsync(context.HttpContext.Response.Body);
        }
    }
}