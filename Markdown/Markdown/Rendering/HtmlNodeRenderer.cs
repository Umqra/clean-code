using System.Text;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    public class HtmlNodeRenderer : INodeRenderer
    {
        public string Visit(ParagraphNode node)
        {
            return VisitInternalNode(node, "p");
        }

        public string Visit(BoldTextNode node)
        {
            return VisitInternalNode(node, "strong");
        }

        public string Visit(ItalicTextNode node)
        {
            return VisitInternalNode(node, "em");
        }

        public string Visit(TextNode node)
        {
            return node.Text;
        }

        private string VisitInternalNode(IInternalNode node, string tagName)
        {
            var innerHtml = new StringBuilder();
            foreach (var child in node.Children)
                innerHtml.Append(child.Accept(this));
            return $"<{tagName}>{innerHtml}</{tagName}>";
        }
    }
}