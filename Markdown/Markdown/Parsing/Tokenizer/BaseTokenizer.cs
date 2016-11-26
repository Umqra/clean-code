using System;

namespace Markdown.Parsing.Tokenizer
{
    public abstract class BaseTokenizer<T> : ITokenizer<T> where T : class
    {
        protected string Text { get; }
        protected int TextPosition { get; }
        protected char CurrentSymbol => Text[TextPosition];

        protected BaseTokenizer(string text, int textPosition = 0)
        {
            Text = text;
            TextPosition = textPosition;
        }

        public abstract T CurrentToken { get; }

        public bool AtEnd => TextPosition == Text.Length;
        public abstract ITokenizer<T> Advance();

        protected string LookAtString(int length)
        {
            // CR (krait): Надо как-то переделать всю инфраструктуру, чтобы избежать постоянных ToString-ов.
            // CR (krait): Обрати внимание на string.Compare.
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, length));
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