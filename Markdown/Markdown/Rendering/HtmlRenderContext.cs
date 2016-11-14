using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class HtmlRenderContext : BaseTreeContext
    {
        private readonly StringBuilder htmlMarkup;
        private readonly INodeToHtmlEntityConverter nodeConverter;

        public string HtmlMarkup => htmlMarkup.ToString();

        public HtmlRenderContext(INodeToHtmlEntityConverter nodeConverter)
        {
            this.nodeConverter = nodeConverter;
            htmlMarkup = new StringBuilder();
        }

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