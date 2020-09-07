using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Zaabee.Extensions;
using Zaabee.ZeroFormatter;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public class ZeroInputFormatter : InputFormatter
    {
        public ZeroInputFormatter(MediaTypeHeaderValue contentType) => SupportedMediaTypes.Add(contentType);

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var bytes = await context.HttpContext.Request.Body.ReadToEndAsync();
            var result = ZeroSerializer.Deserialize(context.ModelType, bytes);
            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}