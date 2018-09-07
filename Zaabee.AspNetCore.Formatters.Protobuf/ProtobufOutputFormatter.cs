using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ProtoBuf.Meta;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        private static readonly Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

        private static RuntimeTypeModel Model => model.Value;

        public ProtobufOutputFormatter(MediaTypeHeaderValue contentType)
        {
            SupportedMediaTypes.Add(contentType);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            Model.Serialize(response.Body, context.Object);
            return Task.FromResult(response);
        }

        private static RuntimeTypeModel CreateTypeModel()
        {
            var typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
            return typeModel;
        }
    }
}