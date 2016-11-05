namespace Markdown.Parsing.Tokens
{
    public class CharacterToken : IToken
    {
        public string Text { get; }

        public CharacterToken(char symbol)
        {
            Text = new string(symbol, 1);
        }

        protected bool Equals(CharacterToken other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }
    }
}
