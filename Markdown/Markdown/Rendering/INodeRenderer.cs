using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    public interface INodeRenderer
    {
        string Render(INode node);
    }
}