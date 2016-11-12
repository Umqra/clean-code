using System;

namespace Markdown.Cli
{
    public class CliOptions
    {
        public string InputFilename { get; set; }
        public string OutputFilename { get; set; }

        // CR: In this particular case, exceptions look better.
        // We talked that having exception-free code is usually
        // easier to use, since it has predictable execution flow.
        // However when we have initial validation, that is nither
        // called more that once nor used outside of this solution,
        // and considering the fact that you just mask exceptions behind
        // this interface, I thinks it totally makes sense to switch
        // to exception-based validation.
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