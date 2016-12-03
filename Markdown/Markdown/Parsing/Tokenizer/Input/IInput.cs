using System;

namespace Markdown.Parsing.Tokenizer.Input
{
    public interface IInput : IDisposable
    {
        string LookAtString(int length);
        char? LookAhead(int distance);
        char? LookBehind(int distance);
        IInput Advance(int distance);
        bool AtEnd { get; }
    }
}
