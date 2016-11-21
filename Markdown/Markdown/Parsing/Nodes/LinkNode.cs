using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class LinkNode : IInternalNode
    {
        public string Reference { get; }
        public List<INode> Children { get; }

        public LinkNode(string reference, INode text)
        {
            Reference = reference;
            Children = new List<INode> {text};
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
            unchecked
            {
                return ((Reference?.GetHashCode() ?? 0) * 397) ^
                       (Children?.CombineElementHashCodesUsingParent(this) ?? 0);
            }
        }


        protected bool Equals(LinkNode other)
        {
            return Children.SequenceEqual(other.Children) &&
                   Equals(Reference, other.Reference);
        }
    }
}