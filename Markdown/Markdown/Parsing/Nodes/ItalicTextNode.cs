using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class ItalicTextNode : IInternalNode
    {
        public List<INode> Children { get; set; }

        public ItalicTextNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
