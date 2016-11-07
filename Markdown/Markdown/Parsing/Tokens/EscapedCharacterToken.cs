namespace Markdown.Parsing.Tokens
{
    public class EscapedCharacterToken : IPlainTextToken
    {
        public EscapedCharacterToken(char symbol)
        {
            Text = new string(symbol, 1);
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EscapedCharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Escape({Text})";
        }

        protected bool Equals(EscapedCharacterToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}