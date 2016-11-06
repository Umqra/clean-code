using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Tokens
{
    public class EmphasisModificatorToken : IToken
    {
        public EmphasisModificatorToken(string text)
        {
            Text = text;
        }

        public string Text { get; }

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

        protected bool Equals(EmphasisModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EmphasisModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"Modificator({Text})";
        }
    }
}