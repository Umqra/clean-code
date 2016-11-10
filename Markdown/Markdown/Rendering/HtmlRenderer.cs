using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class HtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public string Render(INode node)
        {
            Context = new HtmlRenderContext(new NodeHtmlRendererFactory());
            Visit(node);
            return Context.HtmlMarkup;
        }
    }
}