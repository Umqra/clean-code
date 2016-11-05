using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Tokens
{
    public class EscapedCharacterToken : IToken
    {
        public char Symbol { get; }

        public EscapedCharacterToken(char symbol)
        {
            Symbol = symbol;
        }

        protected bool Equals(EscapedCharacterToken other)
        {
            return Symbol == other.Symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EscapedCharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }
    }
}
