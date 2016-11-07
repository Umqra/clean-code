using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface IInternalNode : INode
    {
        List<INode> Children { get; }
    }
}