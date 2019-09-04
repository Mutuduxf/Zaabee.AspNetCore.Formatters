using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Demo;
using Jil;
using Newtonsoft.Json;
using Xunit;
using Zaabee.Protobuf;

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
        public void TestProtobuf()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var stream = new MemoryStream();
            stream.PackBy(dtos);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            httpRequestMessage.Headers.Add("Accept", "application/x-protobuf");

            // HTTP POST with Protobuf Request Body
            var response = client.SendAsync(httpRequestMessage).Result;

            var result = ProtobufHelper.Unpack<List<TestDto>>(response.Content.ReadAsStreamAsync().Result);

            Assert.True(CompareDtos(dtos, result));
        }

        [Fact]
        public void TestJil()
        {
            var client = _server.CreateClient();
            var dtos = GetDtos();
            var json = JSON.Serialize(dtos, new Options(dateFormat: DateTimeFormat.ISO8601,
                excludeNulls: true, includeInherited: true,
                serializationNameFormat: SerializationNameFormat.CamelCase));

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-jil")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-jil");

            var response = client.SendAsync(httpRequestMessage).Result;

            var result =
                JSON.Deserialize<List<TestDto>>(response.Content.ReadAsStringAsync()
                    .Result, new Options(dateFormat: DateTimeFormat.ISO8601,
                    excludeNulls: true, includeInherited: true,
                    serializationNameFormat: SerializationNameFormat.CamelCase));

            Assert.True(CompareDtos(dtos, result));
        }

        [Fact]
        public void TestJson()
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
            var response = client.SendAsync(httpRequestMessage).Result;

            var result =
                JsonConvert.DeserializeObject<List<TestDto>>(response.Content.ReadAsStringAsync().Result);

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
                    dtoOne.Tag != dtoTwo.Tag ||
//                    dtoOne.TestTime != dtoTwo.TestTime ||
                    !CompareDtos(dtoOne.Kids, dtoTwo.Kids))
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
                    Kids = new List<TestDto>
                    {
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 1,
                            CreateTime = DateTime.Now,
//                            TestTime = DateTimeOffset.Now,
                            Name = "00",
                            Enum = TestEnum.Banana
                        },
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 2,
                            CreateTime = DateTime.Now,
//                            TestTime = DateTimeOffset.Now,
                            Name = "01",
                            Enum = TestEnum.Pear
                        }
                    }
                },
                new TestDto
                {
                    Id = Guid.NewGuid(),
                    Tag = long.MaxValue - 3,
                    CreateTime = DateTime.Now,
//                    TestTime = DateTimeOffset.Now,
                    Name = "1",
                    Enum = TestEnum.Apple,
                    Kids = new List<TestDto>
                    {
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 4,
                            CreateTime = DateTime.Now,
//                            TestTime = DateTimeOffset.Now,
                            Name = "10",
                            Enum = TestEnum.Banana
                        },
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 5,
                            CreateTime = DateTime.Now,
//                            TestTime = DateTimeOffset.Now,
                            Name = "11",
                            Enum = TestEnum.Pear
                        }
                    }
                }
            };
        }
    }
}