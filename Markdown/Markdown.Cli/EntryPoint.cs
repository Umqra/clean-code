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
                // CR: Also, it's a good practice to exit with
                // non -zero exit code with Environment.Exit
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
                // CR: To avoid working with console in multiple places
                // (because you might want to switch to dedicated logging solution)
                // it's better to throw exception here to let top-level handler do it's job.
                // For example, set exit code to non-zero.
                Console.Error.WriteLine(parsingStatus.ErrorText);
                parser.HelpOption.ShowHelp(parsingStatus.Errors.Select(error => error.Option));
                return;
            }

            var options = parser.Object;
            if (!options.AreValid())
            {
                // CR: Same, it's not a valid situiation, so you might want to
                // set exit-code to non-zero
                parser.HelpOption.ShowHelp(parser.Options);
                return;
            }

            ConvertMarkdownToHtml(options);
        }

        private static void ConvertMarkdownToHtml(CliOptions options)
        {
            var markdownMarkup = File.ReadAllText(options.InputFilename);
            var markdownToHtmlRenderer =
                new MarkdownToHtmlRenderer(
                    new MarkdownParser(),
                    new MarkdownTokenizerFactory(),
                    new NodeHtmlRenderer());
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