namespace Markdown.Parsing.Tokens
{
    public class FormatModificatorToken : IToken
    {
        public static readonly FormatModificatorToken BoldUnderscore = new FormatModificatorToken("__");
        public static readonly FormatModificatorToken ItalicUnderscore = new FormatModificatorToken("_");

        public FormatModificatorToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

        protected bool Equals(FormatModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FormatModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Modificator({Text})";
        }
    }
}