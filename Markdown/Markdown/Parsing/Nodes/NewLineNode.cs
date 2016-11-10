namespace Markdown.Parsing.Nodes
{
    public class NewLineNode : INode
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NewLineNode)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        protected bool Equals(NewLineNode other)
        {
            return true;
        }
    }
}