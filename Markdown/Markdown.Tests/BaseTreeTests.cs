using Markdown.Parsing;
using Markdown.Parsing.Nodes;

namespace Markdown.Tests
{
    internal abstract class BaseTreeTests
    {
        protected INode BoldText(params INode[] nodes)
        {
            return new BoldTextNode(nodes);
        }

        protected INode ItalicText(params INode[] nodes)
        {
            return new ItalicTextNode(nodes);
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