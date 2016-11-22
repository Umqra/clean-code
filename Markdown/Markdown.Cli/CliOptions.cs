using System;

namespace Markdown.Cli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }
        public string BaseUrl { get; set; }
        public string HtmlFilename { get; set; }
        public string InjectedHtmlElement { get; set; }

        public CliOptions TryInitialize()
        {
            TryInitializeInputFile();
            TryInitializeOutputFile();
            TryInitializeHtmlFile();
            return this;
        }

        private void TryInitializeHtmlFile()
        {
            if (HtmlFilename != null)
            {
                if (InjectedHtmlElement == null)
                    throw new ArgumentException(
                        "InjectedField must be specified if used HtmlFilename. It can be specified with --inject_el option"
                    );
                try
                {
                    FileExtensions.TryGetReadAccess(HtmlFilename);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException($"Can't read from html file {HtmlFilename}", exception);
                }
            }
        }

        private void TryInitializeInputFile()
        {
            try
            {
                FileExtensions.TryGetReadAccess(InputFilename);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Can't read from input file {InputFilename}", exception);
            }
        }

        private void TryInitializeOutputFile()
        {
            try
            {
                FileExtensions.TryGetWriteAccess(InputFilename);
            }
            catch (Exception exception)
            {
                throw new ArgumentException($"Can't write to output file {OutputFilename}", exception);
            }
        }
    }
}