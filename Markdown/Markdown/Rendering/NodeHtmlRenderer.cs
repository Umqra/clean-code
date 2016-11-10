using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class NodeHtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public string Render(INode node)
        {
            Context = new HtmlRenderContext(new NodeToHtmlEntityConverter());
            Visit(node);
            return Context.HtmlMarkup;
        }
    }
}