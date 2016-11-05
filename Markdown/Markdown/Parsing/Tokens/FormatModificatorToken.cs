using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Tokens
{
    public class FormatModificatorToken : IToken
    {
        public static readonly FormatModificatorToken BoldUnderscore = new FormatModificatorToken("__");
        public static readonly FormatModificatorToken ItalicUnderscore = new FormatModificatorToken("_");

        public string Text { get; }

        public FormatModificatorToken(string text)
        {
            Text = text;
        }

        protected bool Equals(FormatModificatorToken other)
        {
            return string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FormatModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }
    }
}
