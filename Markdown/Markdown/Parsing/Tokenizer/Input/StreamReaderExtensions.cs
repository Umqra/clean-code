using System.IO;
using System.Linq;

namespace Markdown.Parsing.Tokenizer.Input
{
    public static class StreamReaderExtensions
    {
        public static string ReadString(this StreamReader stream, int length)
        {
            char[] buffer = new char[length];
            stream.Read(buffer, 0, length);
            return new string(buffer.TakeWhile(c => c != 0).ToArray());
        }
    }
}