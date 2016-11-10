namespace Markdown.Parsing.Tokens
{
    public class MdNewLineToken : IMdToken
    {
        public MdNewLineToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdNewLineToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }


        protected bool Equals(MdNewLineToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}