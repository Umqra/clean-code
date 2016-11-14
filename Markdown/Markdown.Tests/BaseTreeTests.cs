using Markdown.Parsing.Nodes;

namespace Markdown.Tests
{
    internal abstract class BaseTreeTests
    {
        protected INode StrongModificator(params INode[] nodes)
        {
            return new StrongModificatorNode(nodes);
        }

        protected INode EmphasisModificator(params INode[] nodes)
        {
            return new EmphasisModificatorNode(nodes);
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
            return new CodeModificatorNode(nodes);
        }
    }
}