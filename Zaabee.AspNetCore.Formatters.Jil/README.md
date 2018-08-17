# Zaabee.AspNetCore.Protobuf

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

Now you can send a request with content-type header "application/x-Jil" to get a json format response.

### Demo

Create an asp.net core project and import the Zaabee.AspNetCore.Formatters.Jil from nuget as above,and add class and enum for test like this

```CSharp
[ProtoContract]
public class TestDto
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string Name { get; set; }
    [ProtoMember(3)] public DateTime CreateTime { get; set; }
    [ProtoMember(4)] public List<TestDto> Kids { get; set; }
    [ProtoMember(5)] public long Tag { get; set; }
    [ProtoMember(6)] public TestEnum Enum { get; set; }
}

public enum TestEnum
{
    Apple,
    Banana,
    Pear
}
```

Modify the default controller which named ValuesController to return the dto

```CSharp
[Route("api/[controller]")]
public class ValuesController : Controller
{
    [HttpPost]
    public IEnumerable<TestDto> Post([FromBody]IEnumerable<TestDto> dtos)
    {
        return dtos;
    }
}
```

### Test Project

Create an XUnit project and get Microsoft.AspNetCore.TestHost from nuget

    Install-Package Microsoft.AspNetCore.TestHost
    Install-Package Microsoft.AspNetCore.Diagnostics
    Install-Package Microsoft.AspNetCore.HttpsPolicy
    Install-Package Microsoft.AspNetCore.Mvc

Reference the Demo project and add a test class like this

```CSharp
public class AspNetCoreProtobufTest
{
    private readonly HttpClient _client;

    public AspNetCoreProtobufTest()
    {
        _client = new TestServer(new WebHostBuilder().UseStartup<Startup>()).CreateClient();
    }

    [Fact]
    public async Task Test()
    {
        var dtos = GetDtos();
        var json = JSON.Serialize(dtos);

        var httpContent = new StringContent(json, Encoding.UTF8, "application/x-jil");

        // HTTP POST with Json Request Body
        var responseForPost = _client.PostAsync("api/Values", httpContent);

        var result =
            JsonConvert.DeserializeObject<List<TestDto>>(responseForPost.Result.Content.ReadAsStringAsync()
                .Result);

        Assert.True(CompareDtos(dtos, result));
    }
}
```

You can run or debug the test in Rider(Cross-platform .NET IDE by JetBrains) or Visual Studio with Test Explorer.
