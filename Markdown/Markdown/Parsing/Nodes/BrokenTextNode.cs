namespace Markdown.Parsing.Nodes
{
    public class BrokenTextNode : INode
    {
        public string Text { get; }
        public string Reason { get; }

        public BrokenTextNode(string text, string reason)
        {
            Text = text;
            Reason = reason;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BrokenTextNode)obj);
        }

        public override int GetHashCode()
        {
            return Text != null ? Text.GetHashCode() : 0;
        }

        protected bool Equals(BrokenTextNode other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}