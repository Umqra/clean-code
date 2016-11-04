﻿using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsing.Nodes
{
    public class ItalicTextNode : IInternalNode
    {
        public ItalicTextNode(IEnumerable<INode> children)
        {
            Children = children.ToList();
        }

        public List<INode> Children { get; }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        protected bool Equals(ItalicTextNode other)
        {
            return Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ItalicTextNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.CombineElementHashCodesUsingParent(this) ?? 0;
        }
    }
}