using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Demo;
using Jil;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Zaabee.Jil;
using Zaabee.Protobuf;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchMarkTest>();
            Console.ReadLine();
        }
    }

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Throughput, targetCount: 1000)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class BenchMarkTest
    {
        private static TestServer _server;
        private static HttpClient _jilHttpClient;
        private static HttpClient _protobufHttpClient;
        private static HttpClient _jsonHttpClient;
        private static List<TestDto> _dtos;

        public BenchMarkTest()
        {
            _server = _server ?? new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _jilHttpClient = _jilHttpClient ?? _server.CreateClient();
            _protobufHttpClient = _protobufHttpClient ?? _server.CreateClient();
            _jsonHttpClient = _jsonHttpClient ?? _server.CreateClient();
            _dtos = _dtos ?? GetDtos();
        }

        [Benchmark]
        public void JilPost()
        {
            var json = JilHelper.Serialize(_dtos, new Options(dateFormat: DateTimeFormat.ISO8601,
                excludeNulls: true, includeInherited: true,
                serializationNameFormat: SerializationNameFormat.CamelCase));

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-jil")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-jil");

            var response = _jilHttpClient.SendAsync(httpRequestMessage).Result;

            var result =
                JilHelper.Deserialize<List<TestDto>>(response.Content.ReadAsStringAsync()
                    .Result, new Options(dateFormat: DateTimeFormat.ISO8601,
                    excludeNulls: true, includeInherited: true,
                    serializationNameFormat: SerializationNameFormat.CamelCase));
        }

        [Benchmark]
        public void ProtobufPost()
        {
            var stream = new MemoryStream();
            ProtobufHelper.Serialize(stream, _dtos);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            httpRequestMessage.Headers.Add("Accept", "application/x-protobuf");

            // HTTP POST with Protobuf Request Body
            var responseForPost = _protobufHttpClient.SendAsync(httpRequestMessage);

            var result = ProtobufHelper.Deserialize<List<TestDto>>(
                responseForPost.Result.Content.ReadAsStreamAsync().Result);
        }

        [Benchmark]
        public void JsonPost()
        {
            var json = JsonConvert.SerializeObject(_dtos);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Add("Accept", "application/json");

            // HTTP POST with Json Request Body
            var responseForPost = _jsonHttpClient.SendAsync(httpRequestMessage);

            var result =
                JsonConvert.DeserializeObject<List<TestDto>>(responseForPost.Result.Content.ReadAsStringAsync().Result);
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