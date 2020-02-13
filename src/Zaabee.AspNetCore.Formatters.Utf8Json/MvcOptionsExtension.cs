using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Utf8Json;

namespace Zaabee.AspNetCore.Formatters.Utf8Json
{
    public static class MvcOptionsExtension
    {
        public static void AddUtf8JsonFormatter(this MvcOptions options, string contentType = "application/x-utf8json",
            string format = "utf8json", IJsonFormatterResolver resolver = null)
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment) contentType).CopyAsReadOnly();
            options.InputFormatters.Add(new Utf8JsonInputFormatter(mediaTypeHeaderValue, resolver));
            options.OutputFormatters.Add(new Utf8JsonOutputFormatter(mediaTypeHeaderValue, resolver));
            options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
        }
    }
}