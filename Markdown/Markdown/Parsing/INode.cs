namespace Markdown.Parsing
{
    public interface INode
    {
        T Accept<T>(INodeVisitor<T> visitor);
    }
}