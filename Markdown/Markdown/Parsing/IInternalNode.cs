using System.Collections.Generic;

namespace Markdown.Parsing
{
    public interface IInternalNode : INode
    {
        List<INode> Children { get; }
    }
}