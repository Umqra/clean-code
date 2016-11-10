namespace Markdown.Parsing.Tokens
{
    public class MdEmphasisModificatorToken : IMdToken
    {
        public MdEmphasisModificatorToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdEmphasisModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Emphasis({Text})";
        }

        protected bool Equals(MdEmphasisModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}