using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface INodeVisitor<out T>
    {
        T Visit(ParagraphNode node);
        T Visit(BoldTextNode node);
        T Visit(ItalicTextNode node);
        T Visit(TextNode node);
    }
}