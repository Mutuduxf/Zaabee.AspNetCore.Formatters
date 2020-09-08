using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zaabee.AspNetCore.Formatters.ZeroFormatter
{
    public static class MvcBuilderExtension
    {
        public static IMvcBuilder AddZeroFormatter(this IMvcBuilder mvcBuilder,
            string contentType = "application/x-zeroformatter",
            string format = "zeroformatter")
        {
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));
            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException(nameof(format));

            mvcBuilder.AddMvcOptions(options => { options.AddZeroFormatter(contentType, format); });

            return mvcBuilder;
        }
    }
}