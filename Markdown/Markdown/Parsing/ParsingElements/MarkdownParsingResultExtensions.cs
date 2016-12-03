using System;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.ParsingElements
{
    public static class MarkdownParsingResultExtensions
    {
        public static MarkdownParsingResult<T> IfFail<T>(this MarkdownParsingResult<T> result, 
            Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> parser)
        {
            if (result.Succeed)
                return result;
            return parser(result.Remainder);
        }
    }
}