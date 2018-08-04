using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using ProtoBuf.Meta;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufInputFormatter : InputFormatter
    {
        private static readonly Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        private static RuntimeTypeModel Model => model.Value;

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue.TryParse(request.ContentType, out _);
 
            var result = Model.Deserialize(context.HttpContext.Request.Body, null, type);
            return InputFormatterResult.SuccessAsync(result);
        }
 
        public override bool CanRead(InputFormatterContext context)
        {
            return true;
        }
 
        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}