using System;

namespace Markdown.Cli
{
    public class ParseArgumentException : ArgumentException
    {
        public ParseArgumentException(string message) : base(message)
        {
        }

        public ParseArgumentException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}