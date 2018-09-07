using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufInputFormatter : InputFormatter
    {
        private static readonly Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        private static RuntimeTypeModel Model => model.Value;

        public ProtobufInputFormatter(MediaTypeHeaderValue contentType)
        {
            SupportedMediaTypes.Add(contentType);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var result = Model.Deserialize(context.HttpContext.Request.Body, null, context.ModelType);
            return InputFormatterResult.SuccessAsync(result);
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}