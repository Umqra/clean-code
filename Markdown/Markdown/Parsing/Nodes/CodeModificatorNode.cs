using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Nodes
{
    public class CodeModificatorNode : IInternalNode
    {
        public CodeModificatorNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public List<INode> Children { get; }

        protected bool Equals(CodeModificatorNode other)
        {
            return Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodeModificatorNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }
    }
}
