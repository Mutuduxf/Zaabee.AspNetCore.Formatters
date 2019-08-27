using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.MsgPack
{
    public static class MvcOptionsExtension
    {
        public static void AddMsgPackFormatter(this MvcOptions options, string contentType = "application/x-msgpack",
            string format = "msgpack")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment) contentType).CopyAsReadOnly();

            options.InputFormatters.Add(new MsgPackInputFormatter(mediaTypeHeaderValue));
            options.OutputFormatters.Add(new MsgPackOutputFormatter(mediaTypeHeaderValue));
            options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
        }
    }
}