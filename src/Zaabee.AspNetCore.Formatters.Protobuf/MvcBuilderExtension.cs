using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public static class MvcBuilderExtension
    {
        public static IMvcBuilder AddProtobuf(this IMvcBuilder mvcBuilder, string contentType = "application/x-protobuf",
            string format = "protobuf")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            mvcBuilder.AddMvcOptions(options => { options.AddProtobufFormatter(contentType, format); });

            return mvcBuilder;
        }
    }
}