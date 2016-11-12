using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class NodeHtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public string Render(INode node)
        {
            // Nit: I think it would be safer to initialize context in the constructor
            Context = new HtmlRenderContext(new NodeToHtmlEntityConverter());
            Visit(node);
            return Context.HtmlMarkup;
        }
    }
}