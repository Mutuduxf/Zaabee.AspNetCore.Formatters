using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Xunit;
using Zaabee.Utf8Json;

namespace TestProject
{
    public partial class FormatterTest
    {
        [Fact]
        public async Task TestUtf8Json()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var json = Utf8JsonSerializer.SerializeToJson(dtos, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-utf8json")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-utf8json");

            var response = await client.SendAsync(httpRequestMessage);

            var result =
                Utf8JsonSerializer.Deserialize<List<TestDto>>(await response.Content.ReadAsStringAsync(), null);

            Assert.True(CompareDtos(dtos, result));
        }
    }
}