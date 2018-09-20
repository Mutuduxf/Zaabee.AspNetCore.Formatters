# Zaabee.AspNetCore.Formatters

Formatters for asp.net core

[ProtobufFormatters](https://github.com/Mutuduxf/Zaabee.AspNetCore.Formatters/tree/master/Zaabee.AspNetCore.Formatters.Protobuf)
[JilFormatters](https://github.com/Mutuduxf/Zaabee.AspNetCore.Formatters/tree/master/Zaabee.AspNetCore.Formatters.Jil)

## Benchmark

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.254 (1803/April2018Update/Redstone4)

Intel Core i7-6600U CPU 2.60GHz (Max: 0.80GHz) (Skylake), 1 CPU, 4 logical and 2 physical cores

Frequency=2742190 Hz, Resolution=364.6720 ns, Timer=TSC

.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT  [AttachedDebugger]
  Job-PRXXZX : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT

IterationCount=1000  RunStrategy=Throughput

|       Method |     Mean |     Error |    StdDev |   Median |      Min |      Max | Allocated |
|------------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
|      JilPost | 642.0 us | 10.192 us |  96.97 us | 652.6 us | 416.1 us | 897.8 us |  16.96 KB |
| ProtobufPost | 617.4 us |  9.846 us |  93.72 us | 615.9 us | 429.9 us | 898.2 us |  14.09 KB |
|     JsonPost | 651.2 us | 12.014 us | 114.88 us | 641.3 us | 462.8 us | 997.7 us |  22.56 KB |

  Mean      : Arithmetic mean of all measurements

  Error     : Half of 99.9% confidence interval

  StdDev    : Standard deviation of all measurements

  Median    : Value separating the higher half of all measurements (50th percentile)

  Min       : Minimum

  Max       : Maximum

  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  
  1 us      : 1 Microsecond (0.000001 sec)