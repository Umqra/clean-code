using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class NodeHtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public NodeHtmlRenderer()
        {
            Context = new HtmlRenderContext(new NodeToHtmlEntityConverter());
        }

        public string Render(INode node)
        {
            Visit(node);
            return Context.HtmlMarkup;
        }
    }
}