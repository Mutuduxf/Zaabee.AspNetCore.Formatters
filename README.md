# Zaabee.AspNetCore.Formatters

Formatters for asp.net core

[ProtobufFormatters](https://github.com/Mutuduxf/Zaabee.AspNetCore.Formatters/tree/master/Zaabee.AspNetCore.Formatters.Protobuf)

[JilFormatters](https://github.com/Mutuduxf/Zaabee.AspNetCore.Formatters/tree/master/Zaabee.AspNetCore.Formatters.Jil)

## Benchmark

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.1.100
  [Host]     : .NET Core 2.1.14 (CoreCLR 4.6.28207.04, CoreFX 4.6.28208.01), X64 RyuJIT
  Job-JJZMXO : .NET Core 2.1.14 (CoreCLR 4.6.28207.04, CoreFX 4.6.28208.01), X64 RyuJIT

IterationCount=10000  RunStrategy=Throughput

|            Method |     Mean |   Error |    StdDev |   Median |      Min |        Max | Allocated |
|------------------ |---------:|--------:|----------:|---------:|---------:|-----------:|----------:|
|           JilPost | 490.6 us | 2.35 us |  70.48 us | 474.8 us | 372.9 us |   710.4 us |   8.91 KB |
|      ProtobufPost | 683.0 us | 3.46 us | 104.21 us | 654.5 us | 514.3 us | 1,008.8 us |   8.23 KB |
|          JsonPost | 531.7 us | 2.66 us |  79.88 us | 514.4 us | 393.6 us |   782.3 us |  10.13 KB |
|       MsgPackPost | 453.3 us | 1.96 us |  58.73 us | 440.2 us | 332.4 us |   633.5 us |   7.66 KB |
|      Utf8JsonPost | 452.2 us | 2.13 us |  64.02 us | 438.0 us | 334.4 us |   652.4 us |   7.75 KB |
| ZeroFormatterPost | 412.3 us | 1.68 us |  50.23 us | 404.2 us | 315.6 us |   568.2 us |   7.38 KB |
