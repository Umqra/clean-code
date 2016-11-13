using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class NodeHtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public NodeHtmlRenderer(HtmlRenderContext context) : base(context)
        {
        }

        public string Render(INode node)
        {
            Visit(node);
            return Context.HtmlMarkup;
        }
    }
}