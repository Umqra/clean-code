using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public interface ITokenizer<out T> where T : class
    {
        T CurrentToken { get; }
        bool AtEnd { get; }
        ITokenizer<T> Advance();
    }
}