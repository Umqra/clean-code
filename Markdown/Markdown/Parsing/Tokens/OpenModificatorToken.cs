using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Tokens
{
    public class OpenModificatorToken : IToken
    {
        public string Modificator { get; }

        public OpenModificatorToken(string modificator)
        {
            Modificator = modificator;
        }

        protected bool Equals(OpenModificatorToken other)
        {
            return string.Equals(Modificator, other.Modificator);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OpenModificatorToken)obj);
        }

        public override int GetHashCode()
        {
            return Modificator?.GetHashCode() ?? 0;
        }
    }
}
