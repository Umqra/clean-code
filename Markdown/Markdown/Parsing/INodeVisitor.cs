using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface INodeVisitor
    {
        void Visit(INode node);
    }
}