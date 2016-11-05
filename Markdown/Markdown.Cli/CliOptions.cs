using System;

namespace Markdown.Cli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }

        public bool AreValid()
        {
            if (!FileExtensions.HaveReadAccess(InputFilename))
            {
                Console.Error.WriteLine($"-i, --input: can't read from input file {InputFilename}");
                return false;
            }

            if (!FileExtensions.HaveWriteAccess(OutputFilename))
            {
                Console.Error.WriteLine($"-o, --output: can't write to output file {OutputFilename}");
                return false;
            }

            return true;
        }
    }
}