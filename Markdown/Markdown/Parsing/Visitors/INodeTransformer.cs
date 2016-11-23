using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public interface INodeTransformer
    {
        void Transform(INode node);
    }
}