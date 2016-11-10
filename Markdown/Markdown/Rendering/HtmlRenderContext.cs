using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class HtmlRenderContext : ATreeContext
    {
        private readonly INodeHtmlRendererFactory rendererFactory;

        public HtmlRenderContext(INodeHtmlRendererFactory rendererFactory)
        {
            this.rendererFactory = rendererFactory;
            htmlMarkup = new StringBuilder();
        }

        private StringBuilder htmlMarkup { get; }
        public string HtmlMarkup => htmlMarkup.ToString();

        protected override void EnterLeafNode(INode node)
        {
            htmlMarkup.Append(rendererFactory.CreateLeaf(node).Content);
        }

        protected override void EnterInternalNode(INode node)
        {
            htmlMarkup.Append(rendererFactory.CreateInternal(node).OpeningTag);
        }

        protected override void ExitInternalNode(INode node)
        {
            htmlMarkup.Append(rendererFactory.CreateInternal(node).ClosingTag);
        }
    }
}