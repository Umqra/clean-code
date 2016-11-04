namespace Markdown.Parsing.Nodes
{
    public class TextNode : INode
    {
        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}