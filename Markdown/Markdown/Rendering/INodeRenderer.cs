using Markdown.Parsing;

namespace Markdown.Rendering
{
    public interface INodeRenderer : INodeVisitor<string>
    {
    }
}