using System;
using Jil;
using Microsoft.Extensions.DependencyInjection;

namespace Zaabee.AspNetCore.Formatters.Jil
{
    public static class MvcBuilderExtension
    {
        public static IMvcBuilder AddJil(this IMvcBuilder mvcBuilder, string contentType = "application/x-jil",
            string format = "jil", Options jilOptions = null)
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            mvcBuilder.AddMvcOptions(options => { options.AddJilFormatter(contentType, format, jilOptions); });

            return mvcBuilder;
        }
    }
}