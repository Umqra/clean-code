using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Tokens
{
    public class MdToken : IMdToken
    {
        private SortedSet<Md> Attributes { get; }
        public string UnderlyingText { get; }
        public string Text { get; }

        public MdToken(string text, string underlyingText)
        {
            Text = text;
            UnderlyingText = underlyingText;
            Attributes = new SortedSet<Md>();
        }

        public MdToken(string text) : this(text, text)
        {
        }

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Attributes?.CombineElementHashCodes() ?? 0) * 397) ^ (Text?.GetHashCode() ?? 0);
            }
        }

        protected bool Equals(MdToken other)
        {
            return Attributes.SetEquals(other.Attributes) && string.Equals(Text, other.Text);
        }
    }
}