using System;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MdTokenizerAction
    {
        private Func<ITokenizer<IMdToken>, ITokenizer<IMdToken>> Act { get; }

        public MdTokenizerAction(Func<ITokenizer<IMdToken>, ITokenizer<IMdToken>> act)
        {
            Act = act;
        }

        public ITokenizer<IMdToken> Do(ITokenizer<IMdToken> tokenizer)
        {
            return Act(tokenizer);
        }

        public static MdTokenizerAction SkipTokens(params Md[] attributes)
        {
            return new MdTokenizerAction(tokenizer => tokenizer.UntilMatch(token => token.HasAny(attributes)).Remainder);
        }

        public static MdTokenizerAction BoundTokenizer(Predicate<IMdToken> boundCondition)
        {
            return new MdTokenizerAction(tokenizer => tokenizer.UntilNotMatch(boundCondition));
        }

        public static MdTokenizerAction UnboundTokenizer()
        {
            return new MdTokenizerAction(tokenizer => tokenizer.UnboundTokenizer());
        }

        public static MdTokenizerAction Parse(Func<ITokenizer<IMdToken>, ITokenizer<IMdToken>> act)
        {
            return new MdTokenizerAction(act);
        }
    }
}
