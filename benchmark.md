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
        Method | Length |        Mean |    StdErr |    StdDev |      Median | Scaled | Scaled-StdDev |
-------------- |------- |------------ |---------- |---------- |------------ |------- |-------------- |
      **BaseLine** |    **500** |   **5.6063 ms** | **0.0074 ms** | **0.0210 ms** |   **5.5981 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |   3.1175 ms | 0.1021 ms | 0.3229 ms |   3.0243 ms |   0.56 |          0.05 |
      **BaseLine** |   **5000** |   **5.7063 ms** | **0.0358 ms** | **0.1133 ms** |   **5.6818 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 |  25.2762 ms | 0.1730 ms | 0.4894 ms |  25.2580 ms |   4.43 |          0.11 |
      **BaseLine** |  **50000** |   **5.5990 ms** | **0.0013 ms** | **0.0041 ms** |   **5.6010 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |  50000 | 307.8438 ms | 2.2925 ms | 7.2494 ms | 307.5338 ms |  54.98 |          1.23 |
