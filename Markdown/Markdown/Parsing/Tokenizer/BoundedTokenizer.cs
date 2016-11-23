using System;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.Tokenizer
{
    public class BoundedTokenizer : ITokenizer<IMdToken>
    {
        public ITokenizer<IMdToken> Tokenizer { get; }
        public Predicate<IMdToken> BoundPredicate { get; }

        public IMdToken CurrentToken { get; private set; }

        public bool AtEnd { get; private set; }

        public BoundedTokenizer(ITokenizer<IMdToken> tokenizer, Predicate<IMdToken> boundPredicate)
        {
            Tokenizer = tokenizer;
            BoundPredicate = boundPredicate;
            CurrentToken = Tokenizer.CurrentToken;
            AtEnd = Tokenizer.AtEnd;
            ReinitializeToken();
        }

        private void ReinitializeToken()
        {
            if (CurrentToken != null && !AtEnd && BoundPredicate(CurrentToken))
            {
                CurrentToken = null;
                AtEnd = true;
            }
        }

        public ITokenizer<IMdToken> Advance()
        {
            if (AtEnd)
                return this;
            return new BoundedTokenizer(Tokenizer.Advance(), BoundPredicate);
        }
    }
}