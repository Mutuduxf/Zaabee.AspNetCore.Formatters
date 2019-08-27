using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public static class MvcOptionsExtension
    {
        public static void AddZeroFormatter(this MvcOptions options, string contentType = "application/x-zeroformatter",
            string format = "zeroformatter")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment) contentType).CopyAsReadOnly();

            options.InputFormatters.Add(new ZeroInputFormatter(mediaTypeHeaderValue));
            options.OutputFormatters.Add(new ZeroOutputFormatter(mediaTypeHeaderValue));
            options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
        }
    }
}