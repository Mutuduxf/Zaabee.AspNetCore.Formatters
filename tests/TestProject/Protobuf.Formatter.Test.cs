using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Demo;
using Xunit;
using Zaabee.Protobuf;

namespace TestProject
{
    public partial class FormatterTest
    {
        [Fact]
        public async Task TestProtobuf()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var stream = new MemoryStream();
            ProtobufSerializer.Pack(dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            httpRequestMessage.Headers.Add("Accept", "application/x-protobuf");

            // HTTP POST with Protobuf Request Body
            var response = await client.SendAsync(httpRequestMessage);

            var result = ProtobufSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());

            Assert.True(CompareDtos(dtos, result));
        }
    }
}