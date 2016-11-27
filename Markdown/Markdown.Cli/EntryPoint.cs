using System;
using System.IO;
using System.Linq;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Fclp;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Visitors;
using Markdown.Rendering;
using Markdown.Rendering.HtmlEntities;

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
                if (innerExceptions.Any(e => e is ArgumentParseException))
                    Console.WriteLine("Type /?, -h, --help to call help message");
                Environment.Exit(1);
            }
        }

        private static void RunCli(string[] args)
        {
            var parser = ConfigureParser();

            var parsingStatus = parser.Parse(args);
            if (parsingStatus.HelpCalled) return;
            if (parsingStatus.HasErrors)
                throw new ArgumentParseException(parsingStatus.ErrorText);

            CliOptions options;
            try
            {
                options = parser.Object.TryInitialize();
            }
            catch (Exception exception)
            {
                throw new ArgumentParseException("Invalid cli arguments", exception);
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
            {
                File.WriteAllText(options.OutputFilename, htmlMarkup);
            }
            else
            {
                IHtmlDocument templateDom;
                using (var fileStream = File.OpenRead(options.HtmlFilename))
                {
                    templateDom = new HtmlParser().Parse(fileStream);
                }
                if (templateDom == null)
                    throw new Exception($"Error while insert generated html markup into {options.HtmlFilename}");

                templateDom.QuerySelector(options.InjectedHtmlElement).InnerHtml = htmlMarkup;
                File.WriteAllText(options.OutputFilename, templateDom.DocumentElement.OuterHtml);
            }
        }

        private static INodeRenderer GetNodeRenderer(CliOptions options)
        {
            NodeToHtmlEntityConverter converter;
            if (options.InjectCssClass != null)
                converter = new NodeToHtmlEntityConverter(new HtmlAttribute("class", options.InjectCssClass));
            else
                converter = new NodeToHtmlEntityConverter();
            return new NodeHtmlRenderer(new HtmlRenderContext(converter));
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
                htmlRenderer =
                    htmlRenderer.WithModificators(new TransformTreeVisitor(new BaseUrlTransformer(options.BaseUrl)));
            return htmlRenderer;
        }

        private static FluentCommandLineParser<CliOptions> ConfigureParser()
        {
            var parser = new FluentCommandLineParser<CliOptions>();
            parser
                .Setup(arg => arg.InputFilename)
                .As('i', "input")
                .WithDescription("Input file with markdown markup");

            parser
                .Setup(arg => arg.OutputFilename)
                .As('o', "output")
                .WithDescription("Output file for generated html markup");

            parser
                .Setup(arg => arg.BaseUrl)
                .As("base_url")
                .WithDescription("Base url for relative links");

            parser
                .Setup(arg => arg.HtmlFilename)
                .As("html_file")
                .WithDescription("File with HTML template in which generated markup will be placed. ");

            parser
                .Setup(arg => arg.InjectedHtmlElement)
                .As("inject_element")
                .WithDescription(
                    "HTML DOM element in which generated markup will be injected. " +
                    "Use of this option REQUIRES html_file option specified via command line or config file. " + 
                    "Use css selectors for specifying necessary element. " +
                    "For example: --inject_element #markdown, --inject_element body, --inject_element .markdown_class");

            parser
                .Setup(arg => arg.InjectCssClass)
                .As("class")
                .WithDescription("Css class added to all elements in generated markup. " +
                                 "For example: --class .markdown_element");

            parser
                .Setup(arg => arg.ConfigFilename)
                .As('c', "config")
                .WithDescription("Сonfiguration file in YAML format");

            parser
                .SetupHelp("h", "help", "?")
                .WithHeader(@"USAGE:
    MarkdownCli.exe [-i|--input filename] [-o|--output filename] [--config filename] [--base_url /url] [--inject_element #element] [--class .class]

EXAMPLES:
    MarkdownCli.exe -i Spec.md -o Spec.html --base_url http://google.com/ --inject_element #markdown --class .markdown_el
    MarkdownCli.exe --config configu.yml

CONFIG EXAMPLE:
    # Markdown parser configuration file
    input: Spec.md
    output: Spec.html
    html_file: template.html
    inject_element: ""#markdown""
    class: .markdown_el

OPTIONS:")
                .Callback(Console.Write);
            return parser;
        }
    }
}