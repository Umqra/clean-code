using Markdown.Parsing.Nodes;
using Markdown.Rendering.HtmlEntities;

namespace Markdown.Rendering
{
    public interface INodeHtmlRendererFactory
    {
        IHtmlTag CreateInternal(INode node);
        IHtmlContent CreateLeaf(INode node);
    }
}