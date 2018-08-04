using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public class JilInputFormatter : InputFormatter
    {
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue.TryParse(request.ContentType, out _);

            using (var reader = context.ReaderFactory(request.Body, Encoding.UTF8))
                return InputFormatterResult.SuccessAsync(JSON.Deserialize(reader, context.ModelType));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }
    }
}