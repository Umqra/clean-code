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
        Method | Length |        Mean |    StdErr |     StdDev |      Median | Scaled | Scaled-StdDev |
-------------- |------- |------------ |---------- |----------- |------------ |------- |-------------- |
      **BaseLine** |    **500** |   **5.6475 ms** | **0.0177 ms** |  **0.0559 ms** |   **5.6285 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |   4.8647 ms | 0.0274 ms |  0.0821 ms |   4.8693 ms |   0.86 |          0.02 |
      **BaseLine** |   **5000** |   **5.6012 ms** | **0.0046 ms** |  **0.0138 ms** |   **5.5941 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 |  33.5017 ms | 0.4566 ms |  1.4440 ms |  33.3479 ms |   5.98 |          0.24 |
      **BaseLine** |  **50000** |   **5.6419 ms** | **0.0181 ms** |  **0.0571 ms** |   **5.6133 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |  50000 | 503.7446 ms | 8.0111 ms | 25.3333 ms | 497.5464 ms |  89.29 |          4.34 |
