using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Jil;
using Xunit;
using Zaabee.Jil;

namespace TestProject
{
    public partial class FormatterTest
    {
        [Fact]
        public async Task TestJil()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var options = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, includeInherited: true,
                serializationNameFormat: SerializationNameFormat.CamelCase);
            var json = JilSerializer.SerializeToJson(dtos, options);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-jil")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-jil");

            var response = await client.SendAsync(httpRequestMessage);

            var result = JilSerializer.Deserialize<List<TestDto>>(await response.Content.ReadAsStringAsync(), options);

            Assert.True(CompareDtos(dtos, result));
        }
    }
}