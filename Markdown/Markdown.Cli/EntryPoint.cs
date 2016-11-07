using System;
using System.IO;
using System.Linq;
using Fclp;
using Markdown.Parsing;
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void RunCli(string[] args)
        {
            var parser = ConfigureParser();

            var parsingStatus = parser.Parse(args);
            if (parsingStatus.HelpCalled) return;
            if (parsingStatus.HasErrors)
            {
                Console.Error.WriteLine(parsingStatus.ErrorText);
                parser.HelpOption.ShowHelp(parsingStatus.Errors.Select(error => error.Option));
                return;
            }

            var options = parser.Object;
            if (!options.AreValid())
            {
                parser.HelpOption.ShowHelp(parser.Options);
                return;
            }

            ConvertMarkdownToHtml(options);
        }

        private static void ConvertMarkdownToHtml(CliOptions options)
        {
            var markdownMarkup = File.ReadAllText(options.InputFilename);
            var markdownToHtmlRenderer = new MarkdownRenderer(new MarkdownParser(), new MarkdownTokenizer(), new HtmlNodeRenderer());
            var htmlMarkup = markdownToHtmlRenderer.Render(markdownMarkup);
            File.WriteAllText(options.OutputFilename, htmlMarkup);
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

            parser.SetupHelp("h", "help", "?").Callback(text => Console.WriteLine(text));
            return parser;
        }
    }
}