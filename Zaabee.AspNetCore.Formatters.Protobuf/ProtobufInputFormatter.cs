using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Protobuf;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufInputFormatter : InputFormatter
    {
        public ProtobufInputFormatter(MediaTypeHeaderValue contentType)
        {
            SupportedMediaTypes.Add(contentType);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var result = ProtobufHelper.Deserialize(context.HttpContext.Request.Body, context.ModelType);
            return InputFormatterResult.SuccessAsync(result);
        }
    }
}