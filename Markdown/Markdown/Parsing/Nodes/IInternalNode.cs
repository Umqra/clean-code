using System.Collections.Generic;

namespace Markdown.Parsing.Nodes
{
    public interface IInternalNode : INode
    {
        List<INode> Children { get; }
    }
}