``` ini

BenchmarkDotNet=v0.10.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-4510U CPU 2.00GHz, ProcessorCount=4
Frequency=2533198 Hz, Resolution=394.7579 ns, Timer=TSC
Host Runtime=Clr 4.0.30319.42000, Arch=32-bit RELEASE
GC=Concurrent Workstation
JitModules=clrjit-v4.6.1055.0
Job Runtime(s):
	Clr 4.0.30319.42000, Arch=32-bit RELEASE

Job=TravisBenchmark  TargetCount=10  

```
        Method | Length |       Mean |    StdErr |    StdDev |     Median | Scaled | Scaled-StdDev |
-------------- |------- |----------- |---------- |---------- |----------- |------- |-------------- |
      **BaseLine** |    **500** | **29.8885 ms** | **0.2116 ms** | **0.5986 ms** | **29.7965 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |  4.4253 ms | 0.0782 ms | 0.2473 ms |  4.3353 ms |   0.15 |          0.01 |
      **BaseLine** |   **5000** | **30.6426 ms** | **0.3223 ms** | **1.0191 ms** | **30.7957 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 | 32.3744 ms | 0.4637 ms | 1.4664 ms | 31.9979 ms |   1.06 |          0.06 |
