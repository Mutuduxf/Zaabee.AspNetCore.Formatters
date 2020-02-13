using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Jil;
using Newtonsoft.Json;
using Xunit;
using Zaabee.Jil;
using Zaabee.MsgPack;
using Zaabee.Protobuf;
using Zaabee.Utf8Json;
using Zaabee.ZeroFormatter;

namespace TestProject
{

    public class AspNetCoreFormatterTest
    {
        private readonly TestServer _server;

        public AspNetCoreFormatterTest()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

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

        [Fact]
        public async Task TestJson()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var json = JsonConvert.SerializeObject(dtos);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Add("Accept", "application/json");

            // HTTP POST with Json Request Body
            var response = await client.SendAsync(httpRequestMessage);

            var result = JsonConvert.DeserializeObject<List<TestDto>>(await response.Content.ReadAsStringAsync());

            Assert.True(CompareDtos(dtos, result));
        }

        [Fact]
        public async Task TestMsgPack()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var stream = new MemoryStream();
            MsgPackSerializer.Pack(dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-msgpack");
            httpRequestMessage.Headers.Add("Accept", "application/x-msgpack");

            // HTTP POST with Protobuf Request Body
            var response = await client.SendAsync(httpRequestMessage);

            var result = MsgPackSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());

            Assert.True(CompareDtos(dtos, result));
        }

        [Fact]
        public async Task TestZeroFormatter()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var stream = new MemoryStream();
            ZeroSerializer.Pack(dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-zeroformatter");
            httpRequestMessage.Headers.Add("Accept", "application/x-zeroformatter");

            // HTTP POST with Protobuf Request Body
            var response = await client.SendAsync(httpRequestMessage);

            var result = ZeroSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());

            Assert.True(CompareDtos(dtos, result));
        }

        private static bool CompareDtos(List<TestDto> lstOne, List<TestDto> lstTwo)
        {
            lstOne = lstOne ?? new List<TestDto>();
            lstTwo = lstTwo ?? new List<TestDto>();

            if (lstOne.Count != lstTwo.Count) return false;

            for (var i = 0; i < lstOne.Count; i++)
            {
                var dtoOne = lstOne[i];
                var dtoTwo = lstTwo[i];

                var i1 = dtoOne.Id != dtoTwo.Id;
                var i2 = dtoOne.CreateTime.Ticks != dtoTwo.CreateTime.Ticks;
                var i3 = dtoOne.Enum != dtoTwo.Enum;
                var i4 = dtoOne.Name != dtoTwo.Name;
                var i5 = dtoOne.Tag != dtoTwo.Tag;
//                var i6 = dtoOne.TestTime.Ticks != dtoTwo.TestTime.Ticks;


                if (dtoOne.Id != dtoTwo.Id ||
                    dtoOne.CreateTime.ToUniversalTime() != dtoTwo.CreateTime.ToUniversalTime() ||
                    dtoOne.Enum != dtoTwo.Enum ||
                    dtoOne.Name != dtoTwo.Name ||
                    dtoOne.Tag != dtoTwo.Tag)
//                    dtoOne.TestTime != dtoTwo.TestTime ||
//                    !CompareDtos(dtoOne.Kids, dtoTwo.Kids))
                    return false;
            }

            return true;
        }

        private static List<TestDto> GetDtos()
        {
            return new List<TestDto>
            {
                new TestDto
                {
                    Id = Guid.NewGuid(),
                    Tag = long.MaxValue,
                    CreateTime = DateTime.Now,
//                    TestTime = DateTimeOffset.Now,
                    Name = "0",
                    Enum = TestEnum.Apple,
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 1,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "00",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 2,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "01",
//                            Enum = TestEnum.Pear
//                        }
//                    }
                },
                new TestDto
                {
                    Id = Guid.NewGuid(),
                    Tag = long.MaxValue - 3,
                    CreateTime = DateTime.Now,
//                    TestTime = DateTimeOffset.Now,
                    Name = "1",
                    Enum = TestEnum.Apple,
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 4,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "10",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 5,
//                            CreateTime = DateTime.Now,
////                            TestTime = DateTimeOffset.Now,
//                            Name = "11",
//                            Enum = TestEnum.Pear
//                        }
//                    }
                }
            };
        }
    }
}