namespace Markdown.Parsing.Nodes
{
    public class ParagraphNode : INode
    {
        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
