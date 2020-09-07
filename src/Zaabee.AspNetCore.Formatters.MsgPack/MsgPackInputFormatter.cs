using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Extensions;
using Zaabee.MsgPack;

namespace Zaabee.AspNetCore.Formatters.MsgPack
{
    public class MsgPackInputFormatter : InputFormatter
    {
        public MsgPackInputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var bytes = await context.HttpContext.Request.Body.ReadToEndAsync();
            var result = MsgPackSerializer.Deserialize(context.ModelType, bytes);
            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}