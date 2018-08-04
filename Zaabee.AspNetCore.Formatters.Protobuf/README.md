# Zaabee.AspNetCore.Protobuf

Protobuf formatters for asp.net core

## QuickStart

### NuGet

    Install-Package Zaabee.AspNetCore.Protobuf

### Build Project

Create an asp.net core project and import reference in startup.cs

```CSharp
using Zaabee.AspNetCore.Protobuf;
```

Modify the ConfigureServices like this

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => { options.AddProtobufFormatter(); })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
}
```

Now you can send a request with content-type header "application/x-protobuf" to get a protobuf format response.

### Demo

Create an asp.net core project and import the Zaabee.AspNetCoreProtobuf from nuget as above,and add class and enum for test like this

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
        // Act
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
        var response = await _client.GetAsync("/api/values");
        response.EnsureSuccessStatusCode();

        var result = ProtoBuf.Serializer.Deserialize<List<TestDto>>(await response.Content.ReadAsStreamAsync());

        // Assert
        Assert.Equal("application/x-protobuf", response.Content.Headers.ContentType.MediaType);

        Assert.Equal(long.MaxValue, result[0].Tag);
        Assert.Equal(long.MaxValue-1, result[0].Kids[0].Tag);
        Assert.Equal(long.MaxValue-2, result[0].Kids[1].Tag);

        Assert.Equal("0", result[0].Name);
        Assert.Equal("00", result[0].Kids[0].Name);
        Assert.Equal("01", result[0].Kids[1].Name);
    }
}
```

You can run or debug the test in Rider(Cross-platform .NET IDE by JetBrains) or Visual Studio with Test Explorer.
