namespace Markdown.Parsing.Nodes
{
    public interface INode
    {
        T Accept<T>(INodeVisitor<T> visitor);
    }
}