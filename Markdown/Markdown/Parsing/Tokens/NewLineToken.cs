namespace Markdown.Parsing.Tokens
{
    public class NewLineToken : IToken
    {
        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NewLineToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }


        protected bool Equals(NewLineToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}