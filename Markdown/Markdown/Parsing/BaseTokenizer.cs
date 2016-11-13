using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public abstract class BaseTokenizer<T> : ITokenizer<T> where T : class
    {
        protected string Text { get; }
        protected int TextPosition { get; set; }

        protected bool TextEnded => TextPosition == Text.Length;
        protected char CurrentSymbol => Text[TextPosition];

        protected BaseTokenizer(string text)
        {
            Text = text;
            TextPosition = 0;
        }

        public TSpec TakeTokenIfMatch<TSpec>(Predicate<TSpec> matchPredicate) where TSpec : class, T
        {
            var oldPosition = TextPosition;

            var token = TakeToken<TSpec>();
            if (token != null && matchPredicate(token))
                return token;

            TextPosition = oldPosition;
            return null;
        }

        public TSpec TakeTokenIfMatch<TSpec>() where TSpec : class, T
        {
            return TakeTokenIfMatch<TSpec>(_ => true);
        }

        public List<T> TakeTokensUntilMatch(Predicate<T> matchPredicate)
        {
            var tokens = new List<T>();
            while (true)
            {
                var token = TakeTokenIfMatch(matchPredicate);
                if (token == null)
                    break;
                tokens.Add(token);
            }
            return tokens;
        }

        protected abstract T ParseToken();

        public TSpec TakeToken<TSpec>() where TSpec : class, T
        {
            if (TextEnded)
                return null;
            return ParseToken() as TSpec;
        }

        protected string LookAtString(int length)
        {
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, length));
        }

        protected string TakeString(int length)
        {
            var result = LookAtString(length);
            TextPosition += result.Length;
            return result;
        }

        protected char? LookAhead(int distance)
        {
            if (TextPosition + distance < Text.Length)
                return Text[TextPosition + distance];
            return null;
        }

        protected char? LookBehind(int distance)
        {
            if (TextPosition - distance >= 0)
                return Text[TextPosition - distance];
            return null;
        }
    }
}