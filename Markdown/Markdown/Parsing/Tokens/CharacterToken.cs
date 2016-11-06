namespace Markdown.Parsing.Tokens
{
    public class CharacterToken : IToken
    {
        public CharacterToken(char symbol)
        {
            Text = new string(symbol, 1);
        }

        public string Text { get; }

        protected bool Equals(CharacterToken other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Char({Text})";
        }
    }
}