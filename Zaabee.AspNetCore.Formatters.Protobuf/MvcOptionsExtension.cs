using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Protobuf
{
    public static class MvcOptionsExtension
    {
        public static void AddProtobufFormatter(this MvcOptions options, string contentType = "application/x-protobuf",
            string format = "protobuf")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment) contentType).CopyAsReadOnly();

            options.InputFormatters.Insert(options.InputFormatters.Count, new ProtobufInputFormatter(mediaTypeHeaderValue));
            options.OutputFormatters.Insert(options.OutputFormatters.Count, new ProtobufOutputFormatter(mediaTypeHeaderValue));
            options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
        }
    }
}