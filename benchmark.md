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
        Method | Length |        Mean |    StdDev |      Median | Scaled | Scaled-StdDev |
-------------- |------- |------------ |---------- |------------ |------- |-------------- |
      **BaseLine** |    **500** |   **5.5985 ms** | **0.0109 ms** |   **5.6020 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |   4.4304 ms | 0.0328 ms |   4.4125 ms |   0.79 |          0.01 |
      **BaseLine** |   **5000** |   **5.5924 ms** | **0.0052 ms** |   **5.5939 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 |  33.1947 ms | 0.5769 ms |  33.2030 ms |   5.94 |          0.10 |
      **BaseLine** |  **50000** |   **5.6591 ms** | **0.0643 ms** |   **5.6571 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |  50000 | 439.4388 ms | 5.3324 ms | 439.0637 ms |  77.66 |          1.22 |
