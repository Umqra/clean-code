using Markdown.Parsing.Nodes;
using Markdown.Rendering.HtmlEntities;

namespace Markdown.Rendering
{
    public interface INodeToHtmlEntityConverter
    {
        IHtmlTag ConvertInternal(INode node);
        IHtmlContent ConvertLeaf(INode node);
    }
}