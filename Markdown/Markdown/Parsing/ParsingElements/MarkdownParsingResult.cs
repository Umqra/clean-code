using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.ParsingElements
{
    public class MarkdownParsingResult<TValue>
    {
        public bool Succeed { get; }
        public TValue Parsed { get; }
        public ITokenizer<IMdToken> Remainder { get; }

        private MarkdownParsingResult(bool success, TValue parsed, ITokenizer<IMdToken> remainder)
        {
            Succeed = success;
            Parsed = parsed;
            Remainder = remainder;
        }

        public static MarkdownParsingResult<T> Success<T>(T parsed, ITokenizer<IMdToken> remainder)
        {
            return new MarkdownParsingResult<T>(true, parsed, remainder);
        }

        public static MarkdownParsingResult<T> Fail<T>(ITokenizer<IMdToken> remainder)
        {
            return new MarkdownParsingResult<T>(false, default(T), remainder);
        }
    }
}