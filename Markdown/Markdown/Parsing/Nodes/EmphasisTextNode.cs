using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class EmphasisTextNode : IInternalNode
    {
        public EmphasisTextNode(EmphasisStrength emphasisStrength, IEnumerable<INode> children)
        {
            EmphasisStrength = emphasisStrength;
            Children = children.ToList();
        }

        public EmphasisStrength EmphasisStrength { get; }

        public List<INode> Children { get; }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EmphasisTextNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)EmphasisStrength * 397) ^ (Children?.CombineElementHashCodesUsingParent(this) ?? 0);
            }
        }

        protected bool Equals(EmphasisTextNode other)
        {
            return EmphasisStrength == other.EmphasisStrength && Children.SequenceEqual(other.Children);
        }
    }
}