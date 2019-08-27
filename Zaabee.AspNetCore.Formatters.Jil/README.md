# Zaabee.AspNetCore.Jil

Jil formatters for asp.net core

## QuickStart

### NuGet

Install-Package Zaabee.AspNetCore.Formatters.Jil

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaabee.AspNetCore.Formatters.Jil;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => { options.AddJilFormatter(); })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}
```