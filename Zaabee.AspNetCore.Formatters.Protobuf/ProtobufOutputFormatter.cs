using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Protobuf;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        public ProtobufOutputFormatter(MediaTypeHeaderValue contentType)
        {
            SupportedMediaTypes.Add(contentType);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            ProtobufHelper.Serialize(response.Body, context.Object);
            return Task.CompletedTask;
        }
    }
}