using System;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    public class HtmlRenderer : ContextTreeVisitor<HtmlRenderContext>, INodeRenderer
    {
        public string Render(INode node)
        {
            Context = new HtmlRenderContext(new NodeHtmlRendererFactory());
            return Context.HtmlMarkup;
        }
    }
}