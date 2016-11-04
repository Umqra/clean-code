using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class BoldTextNode : IInternalNode
    {
        public BoldTextNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public List<INode> Children { get; }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        protected bool Equals(BoldTextNode other)
        {
            return Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BoldTextNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }
    }
}