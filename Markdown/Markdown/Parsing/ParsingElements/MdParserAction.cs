using System;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.ParsingElements
{
    public class MdParserAction<T>
    {
        public Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> Act { get; }
        public T Parsed { get; set; }
        public bool Succeed { get; private set; }

        public MdParserAction(Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> act)
        {
            Act = act;
        }

        public ITokenizer<IMdToken> Do(ITokenizer<IMdToken> tokenizer)
        {
            var result = Act(tokenizer);
            Succeed = result.Succeed;
            if (Succeed)
                Parsed = result.Parsed;
            return result.Remainder;
        }
    }

    public static class MdParserAction
    {
        public static MdParserAction<T> Parse<T>(Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> act)
        {
            return new MdParserAction<T>(act);
        }

        public static MdParserAction<IMdToken> ParseToken(params Md[] needAttributes)
        {
            return Parse(tokenizer => tokenizer.Match(token => token.Has(needAttributes)));
        }
    }
}