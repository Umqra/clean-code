namespace Markdown.Parsing.Tokens
{
    public class MdEscapedCharacterToken : IMdPlainTextToken
    {
        public MdEscapedCharacterToken(char symbol)
        {
            Text = new string(symbol, 1);
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdEscapedCharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Escape({Text})";
        }

        protected bool Equals(MdEscapedCharacterToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}