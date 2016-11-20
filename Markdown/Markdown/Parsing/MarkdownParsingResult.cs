using System;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
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

        public MarkdownParsingResult<U> IfSuccess<U>(Func<ITokenizer<IMdToken>, MarkdownParsingResult<U>> parser)
        {
            if (!Succeed)
                return MarkdownParsingResult<U>.Fail<U>(Remainder);
            return parser(Remainder);
        }

        public MarkdownParsingResult<TValue> IfFail(Func<ITokenizer<IMdToken>, MarkdownParsingResult<TValue>> parser)
        {
            if (Succeed)
                return this;
            return parser(Remainder);
        }
    }
}