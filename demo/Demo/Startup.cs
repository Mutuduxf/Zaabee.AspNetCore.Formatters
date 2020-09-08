using Jil;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaabee.AspNetCore.Formatters.Jil;
using Zaabee.AspNetCore.Formatters.MsgPack;
using Zaabee.AspNetCore.Formatters.Protobuf;
using Zaabee.AspNetCore.Formatters.Utf8Json;
using Zaabee.AspNetCore.Formatters.ZeroFormatter;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddJil(jilOptions: new Options(dateFormat: DateTimeFormat.ISO8601,
                    excludeNulls: true, includeInherited: true,
                    serializationNameFormat: SerializationNameFormat.CamelCase))
                .AddMsgPack()
                .AddProtobuf()
                .AddUtf8Json()
                .AddZeroFormatter()
                .AddNewtonsoftJson()
                .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}