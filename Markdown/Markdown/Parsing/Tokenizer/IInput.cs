using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Tokenizer
{
    public interface IInput
    {
        string LookAtString(int length);
        char? LookAhead(int distance);
        char? LookBehind(int distance);
        IInput Advance(int distance);
        bool AtEnd { get; }
    }
}
