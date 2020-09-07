using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.MsgPack;

namespace Zaabee.AspNetCore.Formatters.MsgPack
{
    public class MsgPackOutputFormatter : OutputFormatter
    {
        public MsgPackOutputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            MsgPackSerializer.PackAsync(context.Object, context.HttpContext.Response.Body);
            return Task.CompletedTask;
        }
    }
}