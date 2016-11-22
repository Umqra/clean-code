using System;
using System.IO;
using System.Linq;
using AngleSharp.Parser.Html;
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

            WriteResult(options, htmlMarkup);
        }

        private static void WriteResult(CliOptions options, string htmlMarkup)
        {
            if (options.HtmlFilename == null)
                File.WriteAllText(options.OutputFilename, htmlMarkup);
            else
            {
                var templateDom = new HtmlParser().Parse(File.OpenRead(options.HtmlFilename));
                Console.WriteLine(templateDom.QuerySelector(options.InjectedHtmlElement).OuterHtml);
                templateDom.QuerySelector(options.InjectedHtmlElement).InnerHtml = htmlMarkup;

                File.WriteAllText(options.OutputFilename, templateDom.DocumentElement.OuterHtml);
            }
        }

        private static INodeRenderer GetNodeRenderer(CliOptions options)
        {
            return new NodeHtmlRenderer(new HtmlRenderContext(new NodeToHtmlEntityConverter()));
        }

        private static MarkdownToHtmlRenderer GetRenderer(CliOptions options)
        {
            var nodeRenderer = GetNodeRenderer(options);

            var htmlRenderer = new MarkdownToHtmlRenderer(
                new MarkdownParser(),
                new MarkdownTokenizerFactory(),
                nodeRenderer
            );
            if (options.BaseUrl != null)
                htmlRenderer = htmlRenderer.WithModificators(new TransformTreeVisitor(new BaseUrlTransformer(options.BaseUrl)));
            return htmlRenderer;
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

            parser
                .Setup(arg => arg.HtmlFilename)
                .As("html_file")
                .WithDescription("HTML template file when generated markup will be injected");

            parser
                .Setup(arg => arg.InjectedHtmlElement)
                .As("inject_el")
                .WithDescription(@"Element in HTML DOM in which will be injected generated markup. You can use well-known css-selectors for specifying needed element. For example: --inject_el #markdown, --inject_el body, --inject_el .markdown_class");

            parser.SetupHelp("h", "help", "?").Callback(text => Console.WriteLine(text));
            return parser;
        }
    }
}