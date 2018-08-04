using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public static class MvcOptionsExtension
    {
        public static void AddJilFormatter(this MvcOptions options, string contentType = "application/x-jil",
            string format = "jil")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));
            
            options.InputFormatters.Add(new JilInputFormatter());
            options.OutputFormatters.Add(new JilOutputFormatter(contentType));
            options.FormatterMappings.SetMediaTypeMappingForFormat(format, MediaTypeHeaderValue.Parse(contentType));
        }
    }
}