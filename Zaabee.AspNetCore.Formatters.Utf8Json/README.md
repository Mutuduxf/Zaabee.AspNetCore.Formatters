# Zaabee.AspNetCore.Utf8Json

Utf8Json formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaabee.AspNetCore.Formatters.Utf8Json

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaabee.AspNetCore.Formatters.Utf8Json;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => { options.AddUtf8JsonFormatter(); })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}
```