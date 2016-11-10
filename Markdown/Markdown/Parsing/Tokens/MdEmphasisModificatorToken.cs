using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Tokens
{
    public class MdEmphasisModificatorToken : IMdToken
    {
        public MdEmphasisModificatorToken(string text)
        {
            Text = text;
        }

        public EmphasisStrength EmphasisStrength
        {
            get
            {
                if (Text.Length == 1)
                    return EmphasisStrength.Low;
                if (Text.Length == 2)
                    return EmphasisStrength.Medium;
                if (Text.Length == 3)
                    return EmphasisStrength.High;
                throw new ArgumentException($"Can't determine {nameof(EmphasisStrength)} of token {this}");
            }
        }

        public string Text { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MdEmphasisModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Modificator({Text})";
        }

        protected bool Equals(MdEmphasisModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}