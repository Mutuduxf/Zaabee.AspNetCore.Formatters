using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Protobuf;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        public ProtobufOutputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            ProtobufSerializer.Pack(context.Object, context.HttpContext.Response.Body);
            return Task.CompletedTask;
        }
    }
}