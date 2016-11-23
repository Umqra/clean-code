using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class HeaderNode : IInternalNode
    {
        public int HeaderType { get; }
        public List<INode> Children { get; }

        public HeaderNode(int headerType, IEnumerable<INode> children)
        {
            HeaderType = headerType;
            Children = children.ToList();
        }

        protected bool Equals(HeaderNode other)
        {
            return HeaderType == other.HeaderType && Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HeaderNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (HeaderType * 397) ^ (Children?.CombineElementHashCodesUsingParent(this) ?? 0);
            }
        }
    }
}