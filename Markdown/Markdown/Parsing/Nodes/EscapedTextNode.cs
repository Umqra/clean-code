namespace Markdown.Parsing.Nodes
{
    public class EscapedTextNode : INode
    {
        public string Text { get; }

        public EscapedTextNode(string text)
        {
            Text = text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EscapedTextNode)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        protected bool Equals(EscapedTextNode other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}