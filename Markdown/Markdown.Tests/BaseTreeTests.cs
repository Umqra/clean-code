using Markdown.Parsing;
using Markdown.Parsing.Nodes;

namespace Markdown.Tests
{
    internal abstract class BaseTreeTests
    {
        protected INode HighEmphasisText(params INode[] nodes)
        {
            return new EmphasisTextNode(EmphasisStrength.High, nodes);
        }

        protected INode MediumEmphasisText(params INode[] nodes)
        {
            return new EmphasisTextNode(EmphasisStrength.Medium, nodes);
        }

        protected INode LowEmphasisText(params INode[] nodes)
        {
            return new EmphasisTextNode(EmphasisStrength.Low, nodes);
        }

        protected INode Paragraph(params INode[] nodes)
        {
            return new ParagraphNode(nodes);
        }

        protected INode Text(string text)
        {
            return new TextNode(text);
        }

        protected INode Group(params INode[] nodes)
        {
            return new GroupNode(nodes);
        }
    }
}