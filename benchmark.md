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
        Method | Length |       Mean |    StdErr |    StdDev |     Median | Scaled | Scaled-StdDev |
-------------- |------- |----------- |---------- |---------- |----------- |------- |-------------- |
      **BaseLine** |    **500** |  **5.7948 ms** | **0.0518 ms** | **0.1638 ms** |  **5.7414 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |  5.2297 ms | 0.1456 ms | 0.4604 ms |  5.1466 ms |   0.90 |          0.08 |
      **BaseLine** |   **5000** |  **5.8008 ms** | **0.0156 ms** | **0.0469 ms** |  **5.8038 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 | 39.4559 ms | 0.5613 ms | 1.7750 ms | 39.3688 ms |   6.80 |          0.29 |
