namespace Markdown.Parsing.Nodes
{
    public class BoldTextNode : INode
    {
        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
