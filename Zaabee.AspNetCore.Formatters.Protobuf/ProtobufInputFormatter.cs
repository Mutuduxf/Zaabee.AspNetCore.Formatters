using System.Collections.Generic;
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
            var bytes = new List<byte>();
            var next = context.HttpContext.Request.Body.ReadByte();
            while (next != -1)
            {
                bytes.Add((byte) next);
                next = context.HttpContext.Request.Body.ReadByte();
            }

            var result = ProtobufHelper.Deserialize(bytes.ToArray(), context.ModelType);
            return InputFormatterResult.SuccessAsync(result);
        }
    }
}