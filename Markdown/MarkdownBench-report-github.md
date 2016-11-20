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

Job=FastBenchmark  TargetCount=10  

```
        Method |  Length |          Mean |     StdErr |      StdDev |        Median |
-------------- |-------- |-------------- |----------- |------------ |-------------- |
 **ParseMarkdown** |   **10000** |    **20.0274 ms** |  **0.0129 ms** |   **0.0342 ms** |    **20.0209 ms** |
 **ParseMarkdown** |  **100000** |   **198.4921 ms** |  **0.5638 ms** |   **1.5948 ms** |   **198.1566 ms** |
 **ParseMarkdown** | **1000000** | **2,158.5041 ms** | **49.7598 ms** | **157.3544 ms** | **2,086.6746 ms** |
