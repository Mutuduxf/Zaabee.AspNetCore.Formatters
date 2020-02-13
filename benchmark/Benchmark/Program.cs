using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Demo;
using Jil;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Zaabee.Jil;
using Zaabee.MsgPack;
using Zaabee.Protobuf;
using Zaabee.Utf8Json;
using Zaabee.ZeroFormatter;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchMarkTest>();
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
        private static HttpClient _msgPackClient;
        private static HttpClient _utf8JsonClient;
        private static HttpClient _zeroFormatterClient;
        
        private static List<TestDto> _dtos;

        public BenchMarkTest()
        {
            _server = _server ?? new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _jilHttpClient = _jilHttpClient ?? _server.CreateClient();
            _protobufHttpClient = _protobufHttpClient ?? _server.CreateClient();
            _jsonHttpClient = _jsonHttpClient ?? _server.CreateClient();
            _msgPackClient = _msgPackClient ?? _server.CreateClient();
            _utf8JsonClient = _utf8JsonClient ?? _server.CreateClient();
            _zeroFormatterClient = _zeroFormatterClient ?? _server.CreateClient();
            _dtos = _dtos ?? GetDtos();
        }

        [Benchmark]
        public async Task JilPost()
        {
            var options = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, includeInherited: true,
                serializationNameFormat: SerializationNameFormat.CamelCase);
            var json = JilSerializer.SerializeToJson(_dtos, options);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-jil")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-jil");

            var response = await _jilHttpClient.SendAsync(httpRequestMessage);

            var result = JilSerializer.Deserialize<List<TestDto>>(await response.Content.ReadAsStringAsync(), options);
        }

        [Benchmark]
        public async Task ProtobufPost()
        {
            var stream = new MemoryStream();
            ProtobufSerializer.Pack(_dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            httpRequestMessage.Headers.Add("Accept", "application/x-protobuf");

            // HTTP POST with Protobuf Request Body
            var response = await _protobufHttpClient.SendAsync(httpRequestMessage);

            var result = ProtobufSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());
        }

        [Benchmark]
        public async Task JsonPost()
        {
            var json = JsonConvert.SerializeObject(_dtos);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Add("Accept", "application/json");

            // HTTP POST with Json Request Body
            var response = await _jsonHttpClient.SendAsync(httpRequestMessage);

            var result = JsonConvert.DeserializeObject<List<TestDto>>(await response.Content.ReadAsStringAsync());
        }

        [Benchmark]
        public async Task MsgPackPost()
        {
            var stream = new MemoryStream();
            MsgPackSerializer.Pack(_dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-msgpack");
            httpRequestMessage.Headers.Add("Accept", "application/x-msgpack");

            // HTTP POST with Protobuf Request Body
            var response = await _msgPackClient.SendAsync(httpRequestMessage);

            var result = MsgPackSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());
        }

        [Benchmark]
        public async Task Utf8JsonPost()
        {
            var json = Utf8JsonSerializer.SerializeToJson(_dtos, null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/x-utf8json")
            };
            httpRequestMessage.Headers.Add("Accept", "application/x-utf8json");

            var response = await _utf8JsonClient.SendAsync(httpRequestMessage);

            var result =
                Utf8JsonSerializer.Deserialize<List<TestDto>>(await response.Content.ReadAsStringAsync(), null);
        }

        [Benchmark]
        public async Task ZeroFormatterPost()
        {
            var stream = new MemoryStream();
            ZeroSerializer.Pack(_dtos, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Values/Post")
            {
                Content = new StreamContent(stream)
            };
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-zeroformatter");
            httpRequestMessage.Headers.Add("Accept", "application/x-zeroformatter");

            // HTTP POST with Protobuf Request Body
            var response = await _zeroFormatterClient.SendAsync(httpRequestMessage);

            var result = ZeroSerializer.Unpack<List<TestDto>>(await response.Content.ReadAsStreamAsync());
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
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 1,
//                            CreateTime = DateTime.Now,
//                            Name = "00",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 2,
//                            CreateTime = DateTime.Now,
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
                    Name = "1",
                    Enum = TestEnum.Apple,
//                    Kids = new List<TestDto>
//                    {
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 4,
//                            CreateTime = DateTime.Now,
//                            Name = "10",
//                            Enum = TestEnum.Banana
//                        },
//                        new TestDto
//                        {
//                            Id = Guid.NewGuid(),
//                            Tag = long.MaxValue - 5,
//                            CreateTime = DateTime.Now,
//                            Name = "11",
//                            Enum = TestEnum.Pear
//                        }
//                    }
                }
            };
        }

    }
}