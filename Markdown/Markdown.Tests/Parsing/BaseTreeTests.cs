using Markdown.Parsing.Nodes;

namespace Markdown.Tests.Parsing
{
    internal abstract class BaseTreeTests
    {
        protected INode StrongModifier(params INode[] nodes)
        {
            return new StrongModifierNode(nodes);
        }

        protected INode EmphasisModifier(params INode[] nodes)
        {
            return new EmphasisModifierNode(nodes);
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

        protected INode NewLine()
        {
            return new NewLineNode();
        }

        protected INode Escaped(string text)
        {
            return new EscapedTextNode(text);
        }

        protected INode Code(params INode[] nodes)
        {
            return new CodeModifierNode(nodes);
        }

        protected INode Link(string reference, INode text)
        {
            return new LinkNode(reference, text);
        }

        protected INode Header(int headerType, params INode[] nodes)
        {
            return new HeaderNode(headerType, nodes);
        }
    }
}