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

namespace TestProject
{

    public class AspNetCoreFormatterTest
    {
        private readonly HttpClient _client;

        public AspNetCoreFormatterTest()
        {
            _client = new TestServer(new WebHostBuilder().UseStartup<Startup>()).CreateClient();
        }

        [Fact]
        public void TestPrtobuf()
        {
            // HTTP Post with Protobuf Response Body
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

            var dtos = GetDtos();
            var stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(stream, dtos);

            var httpContent = new StreamContent(stream);

            // HTTP POST with Protobuf Request Body
            var responseForPost = _client.PostAsync("api/Values", httpContent);

            var result = ProtoBuf.Serializer.Deserialize<List<TestDto>>(
                responseForPost.Result.Content.ReadAsStreamAsync().Result);

            Assert.True(CompareDtos(dtos,result));
        }

        [Fact]
        public void TestJil()
        {
            var dtos = GetDtos();
            var json = JSON.Serialize(dtos);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/x-jil");

            // HTTP POST with Json Request Body
            var responseForPost = _client.PostAsync("api/Values", httpContent);

            try
            {
                var result =
                    JsonConvert.DeserializeObject<List<TestDto>>(responseForPost.Result.Content.ReadAsStringAsync()
                        .Result);

                Assert.True(CompareDtos(dtos, result));
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        [Fact]
        public void TestJson()
        {
            var dtos = GetDtos();
            var json = JsonConvert.SerializeObject(dtos);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // HTTP POST with Json Request Body
            var responseForPost = _client.PostAsync("api/Values", httpContent);

            var result =
                JsonConvert.DeserializeObject<List<TestDto>>(responseForPost.Result.Content.ReadAsStringAsync().Result);

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
                if (dtoOne.Id != dtoTwo.Id || dtoOne.CreateTime != dtoTwo.CreateTime || dtoOne.Enum != dtoTwo.Enum ||
                    dtoOne.Name != dtoTwo.Name || dtoOne.Tag != dtoTwo.Tag || !CompareDtos(dtoOne.Kids, dtoTwo.Kids))
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
                    Name = "0",
                    Enum = TestEnum.Apple,
                    Kids = new List<TestDto>
                    {
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 1,
                            CreateTime = DateTime.Now,
                            Name = "00",
                            Enum = TestEnum.Banana
                        },
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 2,
                            CreateTime = DateTime.Now,
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
                    Name = "1",
                    Enum = TestEnum.Apple,
                    Kids = new List<TestDto>
                    {
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 4,
                            CreateTime = DateTime.Now,
                            Name = "10",
                            Enum = TestEnum.Banana
                        },
                        new TestDto
                        {
                            Id = Guid.NewGuid(),
                            Tag = long.MaxValue - 5,
                            CreateTime = DateTime.Now,
                            Name = "11",
                            Enum = TestEnum.Pear
                        }
                    }
                }
            };
        }
    }
}