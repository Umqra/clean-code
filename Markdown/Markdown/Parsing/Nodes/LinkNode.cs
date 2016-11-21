using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class LinkNode : IInternalNode
    {
        public List<INode> Children { get; }

        public LinkNode(INode reference, INode text)
        {
            Children = new List<INode> {reference, text};
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LinkNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }

        protected bool Equals(LinkNode other)
        {
            return Children.SequenceEqual(other.Children);
        }
    }
}