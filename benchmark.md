``` ini

BenchmarkDotNet=v0.10.0
OS=Unix 4.8.7.40807
Processor=?, ProcessorCount=32
Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
Host Runtime=Mono 4.6.2 (Stable 4.6.2.7/08fd525 Mon Nov 14 12:30:00 UTC 2016), Arch=64-bit RELEASE
GC=Concurrent Workstation
JitModules=
Job Runtime(s):
	Mono 4.6.2 (Stable 4.6.2.7/08fd525 Mon Nov 14 12:30:00 UTC 2016), Arch=64-bit RELEASE

Job=TravisBenchmark  TargetCount=10  

```
        Method | Length |          Mean |     StdDev |        Median | Scaled | Scaled-StdDev |
-------------- |------- |-------------- |----------- |-------------- |------- |-------------- |
      **BaseLine** |    **500** |     **5.5831 ms** |  **0.0044 ms** |     **5.5836 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |    15.3097 ms |  0.1862 ms |    15.2618 ms |   2.74 |          0.03 |
      **BaseLine** |   **5000** |     **5.5997 ms** |  **0.0117 ms** |     **5.6039 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 |   108.5766 ms |  0.6210 ms |   108.7886 ms |  19.39 |          0.11 |
      **BaseLine** |  **50000** |     **5.5812 ms** |  **0.0062 ms** |     **5.5844 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |  50000 | 1,525.6263 ms | 15.0472 ms | 1,526.0004 ms | 273.35 |          2.56 |
