using System;
using System.IO;
using System.Linq;
using Fclp;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Visitors;
using Markdown.Rendering;

namespace Markdown.Cli
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            try
            {
                RunCli(args);
            }
            catch (Exception exception)
            {
                var innerExceptions = exception.EnumerateInnerExceptions().ToList();
                Console.WriteLine(string.Join("\n", innerExceptions.Select(e => " - " + e.Message)));
                if (innerExceptions.Any(e => e is ParseArgumentException))
                    Console.WriteLine("Type ?, h, --help to call help message");
                Environment.Exit(1);
            }
        }

        private static void RunCli(string[] args)
        {
            var parser = ConfigureParser();

            var parsingStatus = parser.Parse(args);
            if (parsingStatus.HelpCalled) return;
            if (parsingStatus.HasErrors)
                throw new ParseArgumentException(parsingStatus.ErrorText);

            CliOptions options;
            try
            {
                options = parser.Object.TryInitialize();
            }
            catch (Exception exception)
            {
                throw new ParseArgumentException("Invalid cli arguments", exception);
            }

            ConvertMarkdownToHtml(options);
        }

        private static void ConvertMarkdownToHtml(CliOptions options)
        {
            var markdownMarkup = File.ReadAllText(options.InputFilename);
            MarkdownToHtmlRenderer markdownToHtmlRenderer = GetRenderer(options);
            var htmlMarkup = markdownToHtmlRenderer.Render(markdownMarkup);
            File.WriteAllText(options.OutputFilename, htmlMarkup);
        }

        private static MarkdownToHtmlRenderer GetRenderer(CliOptions options)
        {
            var renderer = new MarkdownToHtmlRenderer(
                new MarkdownParser(),
                new MarkdownTokenizerFactory(),
                new NodeHtmlRenderer(new HtmlRenderContext(new NodeToHtmlEntityConverter()))
            );
            if (options.BaseUrl != null)
                renderer = renderer.WithModificators(new TransformTreeVisitor(new BaseUrlTransformer(options.BaseUrl)));
            return renderer;
        }

        private static FluentCommandLineParser<CliOptions> ConfigureParser()
        {
            var parser = new FluentCommandLineParser<CliOptions>();
            parser
                .Setup(arg => arg.InputFilename)
                .As('i', "input")
                .Required()
                .WithDescription("Input file with markdown markup");

            parser
                .Setup(arg => arg.OutputFilename)
                .As('o', "output")
                .Required()
                .WithDescription("Output file for generated html-markup");

            parser
                .Setup(arg => arg.BaseUrl)
                .As("base_url")
                .WithDescription("Base url for relative links");

            parser.SetupHelp("h", "help", "?").Callback(text => Console.WriteLine(text));
            return parser;
        }
    }
}