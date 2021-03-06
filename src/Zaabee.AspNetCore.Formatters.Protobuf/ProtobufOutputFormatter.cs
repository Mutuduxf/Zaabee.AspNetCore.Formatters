using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Extensions;
using Zaabee.Protobuf;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        public ProtobufOutputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            await ProtobufSerializer.Serialize(context.Object).WriteToAsync(context.HttpContext.Response.Body);
        }
    }
}