using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    public interface INodeHtmlRendererFactory
    {
        IHtmlTag CreateInternal(INode node);
        IHtmlContent CreateLeaf(INode node);
    }
}