using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public interface INodeVisitor
    {
        void Visit(INode node);
    }
}