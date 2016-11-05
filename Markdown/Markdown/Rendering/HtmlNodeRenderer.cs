using System.Text;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    public class HtmlNodeRenderer : INodeRenderer
    {
        public string Visit(ParagraphNode node)
        {
            return WrapInternalNodesInTag(node, "p");
        }

        public string Visit(BoldTextNode node)
        {
            return WrapInternalNodesInTag(node, "strong");
        }

        public string Visit(ItalicTextNode node)
        {
            return WrapInternalNodesInTag(node, "em");
        }

        public string Visit(TextNode node)
        {
            return node.Text;
        }

        public string Visit(GroupNode node)
        {
            return VisitInternalNodes(node);
        }

        private string VisitInternalNodes(IInternalNode node)
        {
            var innerHtml = new StringBuilder();
            foreach (var child in node.Children)
                innerHtml.Append(child.Accept(this));
            return innerHtml.ToString();
        }

        private string WrapInternalNodesInTag(IInternalNode node, string tagName)
        {
            return $"<{tagName}>{VisitInternalNodes(node)}</{tagName}>";
        }
    }
}