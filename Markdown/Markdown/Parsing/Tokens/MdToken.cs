using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Tokens
{
    public class MdToken : IMdToken
    {
        public MdToken(string text)
        {
            Text = text;
            Attributes = new SortedSet<Md>();
        }

        private SortedSet<Md> Attributes { get; }

        public string Text { get; }

        public bool Has(params Md[] attributes)
        {
            return attributes.All(attribute => Attributes.Contains(attribute));
        }

        public IMdToken With(params Md[] attributes)
        {
            foreach (var attribute in attributes)
                Attributes.Add(attribute);
            return this;
        }

        protected bool Equals(MdToken other)
        {
            return Attributes.SetEquals(other.Attributes) && string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MdToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Attributes?.CombineElementHashCodes() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
            }
        }
    }
}