using System;
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

        public string Visit(EmphasisTextNode node)
        {
            if (node.EmphasisStrength == EmphasisStrength.Low)
                return WrapInternalNodesInTag(node, "em");
            if (node.EmphasisStrength == EmphasisStrength.Medium)
                return WrapInternalNodesInTag(node, "strong");
            if (node.EmphasisStrength == EmphasisStrength.High)
                return WrapInternalNodesInTag(node, "b");
            throw new ArgumentException($"Unknown {nameof(EmphasisStrength)}: {node.EmphasisStrength}");
        }

        public string Visit(TextNode node)
        {
            return node.Text;
        }

        public string Visit(GroupNode node)
        {
            return VisitInternalNodes(node);
        }

        public string Visit(NewLineNode node)
        {
            return "<br/>";
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