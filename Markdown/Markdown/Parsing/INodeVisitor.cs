using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface INodeVisitor<out T>
    {
        T Visit(ParagraphNode node);
        T Visit(EmphasisTextNode node);
        T Visit(TextNode node);
        T Visit(GroupNode node);
    }
}