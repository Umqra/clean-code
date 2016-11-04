using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class BoldTextNode : IInternalNode
    {
        public List<INode> Children { get; set; }

        public BoldTextNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
