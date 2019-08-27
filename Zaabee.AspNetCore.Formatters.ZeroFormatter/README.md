# Zaabee.AspNetCore.ZeroFormatter

Protobuf formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaabee.AspNetCore.Formatters.ZeroFormatter

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaabee.AspNetCore.Formatters.ZeroFormatter;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => { options.AddZeroFormatter(); })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}
```
