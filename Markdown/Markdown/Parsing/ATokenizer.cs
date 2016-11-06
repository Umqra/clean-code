using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public abstract class ATokenizer<T> where T : class
    {
        protected ATokenizer(string text)
        {
            Text = text;
            TextPosition = 0;
        }

        public bool TextEnded => TextPosition == Text.Length;
        public char CurrentSymbol => Text[TextPosition];

        protected string Text { get; }
        protected int TextPosition { get; set; }

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

        public string LookAtString(int length)
        {
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, length));
        }

        public string TakeString(int length)
        {
            var result = LookAtString(length);
            TextPosition += result.Length;
            return result;
        }

        protected abstract T ParseToken();

        public T TakeToken()
        {
            if (TextEnded)
                return null;
            return ParseToken();
        }

        public T TakeTokenIfMatch(Predicate<T> matchPredicate)
        {
            if (TextEnded)
                return null;
            var oldPosition = TextPosition;

            var token = TakeToken();
            if (token != null && matchPredicate(token))
                return token;

            TextPosition = oldPosition;
            return null;
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
    }
}