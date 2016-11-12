using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    // Nit: Why ATokenizer? Abstract? Use full name
    public abstract class ATokenizer<T> where T : class
    {
        protected ATokenizer(string text)
        {
            Text = text;
            TextPosition = 0;
        }

        public bool TextEnded => TextPosition == Text.Length;
        public char CurrentSymbol => Text[TextPosition];

        protected string Text { get; set; }
        protected int TextPosition { get; set; }

        // CR: Protected method should be below public ones
        protected abstract T ParseToken();

        public string LookAtString(int length)
        {
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, length));
        }

        public TSpec TakeToken<TSpec>() where TSpec : class, T
        {
            if (TextEnded)
                return null;
            return ParseToken() as TSpec;
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

        public string TakeString(int length)
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