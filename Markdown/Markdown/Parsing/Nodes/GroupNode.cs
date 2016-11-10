using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class GroupNode : IInternalNode
    {
        public GroupNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public List<INode> Children { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GroupNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }

        protected bool Equals(GroupNode other)
        {
            return Children.SequenceEqual(other.Children);
        }
    }
}