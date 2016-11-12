using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class HtmlRenderContext : ATreeContext
    {
        private readonly INodeToHtmlEntityConverter nodeConverter;

        public HtmlRenderContext(INodeToHtmlEntityConverter nodeConverter)
        {
            this.nodeConverter = nodeConverter;
            htmlMarkup = new StringBuilder();
        }

        // Nit: Naming issue, mb make it field, not property?
        private StringBuilder htmlMarkup { get; }
        public string HtmlMarkup => htmlMarkup.ToString();

        protected override void EnterLeafNode(INode node)
        {
            htmlMarkup.Append(nodeConverter.ConvertLeaf(node).Content);
        }

        protected override void EnterInternalNode(INode node)
        {
            htmlMarkup.Append(nodeConverter.ConvertInternal(node).OpeningTag);
        }

        protected override void ExitInternalNode(INode node)
        {
            htmlMarkup.Append(nodeConverter.ConvertInternal(node).ClosingTag);
        }
    }
}