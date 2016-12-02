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
      **BaseLine** |    **500** |  **5.6544 ms** | **0.0377 ms** | **0.1192 ms** |  **5.5883 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |    500 |  4.3232 ms | 0.0063 ms | 0.0188 ms |  4.3252 ms |   0.76 |          0.02 |
      **BaseLine** |   **5000** |  **5.8375 ms** | **0.0584 ms** | **0.1848 ms** |  **5.8785 ms** |   **1.00** |          **0.00** |
 ParseMarkdown |   5000 | 32.7787 ms | 0.1709 ms | 0.5127 ms | 32.5822 ms |   5.62 |          0.19 |
