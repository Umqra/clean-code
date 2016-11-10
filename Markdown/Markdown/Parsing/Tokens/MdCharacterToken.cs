namespace Markdown.Parsing.Tokens
{
    public class MdCharacterToken : IMdPlainTextToken
    {
        public MdCharacterToken(char symbol)
        {
            Text = new string(symbol, 1);
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdCharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Char({Text})";
        }

        protected bool Equals(MdCharacterToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}