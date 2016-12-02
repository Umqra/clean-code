using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class StrongModifierNode : IInternalNode
    {
        public StrongModifierNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public List<INode> Children { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StrongModifierNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }

        protected bool Equals(StrongModifierNode other)
        {
            return Children.SequenceEqual(other.Children);
        }
    }
}