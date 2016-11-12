using System;
using System.IO;

namespace Markdown.Cli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }

        public CliOptions TryInitialize()
        {
            TryInitializeInputFile();
            TryInitializeOutputFile();
            
            return this;
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