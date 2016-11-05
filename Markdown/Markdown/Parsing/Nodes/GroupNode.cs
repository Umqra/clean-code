using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Nodes
{
    public class GroupNode : IInternalNode
    {
        public List<INode> Children { get; }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public GroupNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }
    }
}
