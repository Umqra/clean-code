using Markdown.Parsing;

namespace Markdown.Rendering
{
    interface INodeRenderer : INodeVisitor<string>
    {
    }
}
