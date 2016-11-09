using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    class MarkdownTokenizerFactory : ITokenizerFactory<IToken>
    {
        public ATokenizer<IToken> CreateTokenizer(string text)
        {
            return new MarkdownTokenizer(text);
        }
    }
}
