using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Rendering;

namespace MarkdownBench
{
    [MarkdownExporter]
    [SimpleJob(targetCount: 10, id: "FastBenchmark")]
    public class MarkdownBench
    {
        [Params(10000, 100000, 1000000)]
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

        [Benchmark]
        public string ParseMarkdown() => 
            new MarkdownToHtmlRenderer(Parser, Factory, 
                new NodeHtmlRenderer(new HtmlRenderContext(Converter)))
            .Render(Data);
    }

    internal class EntryPoint
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<MarkdownBench>();
        }
    }
}