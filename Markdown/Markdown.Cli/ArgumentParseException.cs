using System;

namespace Markdown.Cli
{
    public class ArgumentParseException : ArgumentException
    {
        public ArgumentParseException(string message) : base(message)
        {
        }

        public ArgumentParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}