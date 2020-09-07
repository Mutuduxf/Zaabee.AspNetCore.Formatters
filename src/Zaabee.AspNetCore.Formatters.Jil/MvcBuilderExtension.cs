using System;
using Jil;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public static class MvcBuilderExtension
    {
        public static IMvcBuilder AddJil(this IMvcBuilder mvcBuilder, string contentType = "application/x-jil",
            string format = "jil", Options jilOptions = null)
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse((StringSegment) contentType).CopyAsReadOnly();

            mvcBuilder.AddMvcOptions(options =>
            {
                options.InputFormatters.Add(new JilInputFormatter(jilOptions, mediaTypeHeaderValue));
                options.OutputFormatters.Add(new JilOutputFormatter(jilOptions, mediaTypeHeaderValue));
                options.FormatterMappings.SetMediaTypeMappingForFormat(format, mediaTypeHeaderValue);
            });

            return mvcBuilder;
        }
    }
}