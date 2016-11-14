using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Nit: ^^^^^^^^^^
// Use resharper for that (Cleanup Code)

namespace Markdown.Parsing.Tokens
{
    public enum Md
    {
        Open,
        Close,
        Emphasis,
        Strong,
        Code,
        Escaped,
        PlainText,
        NewLine
    }
}
