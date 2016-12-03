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
      **BaseLine** |    **500** |     **5.5628 ms** |  **0.0017 ms** |     **5.5631 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |    14.3852 ms |  0.0825 ms |    14.3530 ms |   2.59 |          0.01 |
      **BaseLine** |   **5000** |     **5.5685 ms** |  **0.0076 ms** |     **5.5696 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 |   101.9765 ms |  0.8821 ms |   101.7477 ms |  18.31 |          0.15 |
      **BaseLine** |  **50000** |     **5.6033 ms** |  **0.0331 ms** |     **5.6053 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |  50000 | 1,435.6812 ms | 14.0063 ms | 1,436.3968 ms | 256.23 |          2.75 |
