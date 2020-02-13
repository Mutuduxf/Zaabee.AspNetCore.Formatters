using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.ZeroFormatter;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public class ZeroInputFormatter : InputFormatter
    {
        public ZeroInputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var result = ZeroSerializer.Unpack(context.ModelType, context.HttpContext.Request.Body);
            return InputFormatterResult.SuccessAsync(result);
        }
    }
}