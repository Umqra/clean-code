namespace Markdown.Parsing.Nodes
{
    public class ItalicTextNode : INode
    {
        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
