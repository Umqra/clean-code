namespace Markdown.Parsing.Tokens
{
    public class MdTextToken : IMdPlainTextToken
    {
        public MdTextToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdTextToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Char({Text})";
        }

        protected bool Equals(MdTextToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}