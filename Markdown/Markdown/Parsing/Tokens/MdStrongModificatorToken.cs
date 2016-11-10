namespace Markdown.Parsing.Tokens
{
    public class MdStrongModificatorToken : IMdToken
    {
        public MdStrongModificatorToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdStrongModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Strong({Text})";
        }

        protected bool Equals(MdStrongModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}