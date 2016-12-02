using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Rendering;

namespace MarkdownBench
{
    [MarkdownExporter]
    [SimpleJob(targetCount: 10, id: "TravisBenchmark")]
    public class TravisBench
    {
        [Params(500, 5000, 50000)]
        public int Length { get; set; }

        public string Data { get; set; }

        public MarkdownParser Parser { get; set; }
        public MarkdownTokenizerFactory Factory { get; set; }
        public NodeToHtmlEntityConverter Converter { get; set; }

        [Setup]
        public void Setup()
        {
            Parser = new MarkdownParser();
            Factory = new MarkdownTokenizerFactory();
            Converter = new NodeToHtmlEntityConverter();

            var random = new Random(42);
            const string sampleSymbols = "abcde__ ";

            var symbols = new char[Length];
            for (int i = 0; i < Length; i++)
                symbols[i] = sampleSymbols[random.Next(sampleSymbols.Length)];
            Data = new string(symbols);
        }

        [Benchmark(Baseline = true)]
        public long BaseLine()
        {
            int count = (int)1e7;
            long sum = 0;
            for (long i = 0; i < count; i++)
                sum += i;
            return sum;
        }

        [Benchmark]
        public string ParseMarkdown() =>
            new MarkdownToHtmlRenderer(Parser, Factory,
                    new NodeHtmlRenderer(new HtmlRenderContext(Converter)))
                .Render(Data);
    }
}