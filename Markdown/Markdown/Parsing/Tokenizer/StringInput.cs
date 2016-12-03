using System;

namespace Markdown.Parsing.Tokenizer
{
    public class StringInput : IInput
    {
        public string Text { get; }
        public int Position { get; }

        public bool AtEnd => Text.Length <= Position;

        public StringInput(string text) : this(text, 0)
        {
        }

        private StringInput(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public string LookAtString(int length)
        {
            return AtEnd ? "" : Text.Substring(Position, Math.Min(Text.Length - Position, length));
        }

        public char? LookAhead(int distance)
        {
            if (Position + distance < Text.Length)
                return Text[Position + distance];
            return null;
        }

        public char? LookBehind(int distance)
        {
            if (Position - distance >= 0)
                return Text[Position - distance];
            return null;
        }

        public IInput Advance(int distance)
        {
            return new StringInput(Text, Position + distance);
        }

        public void Dispose()
        {
        }
    }
}