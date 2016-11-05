using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsing.Tokens
{
    public class CharacterToken : IToken
    {
        public char Symbol { get; }

        public CharacterToken(char symbol)
        {
            Symbol = symbol;
        }

        protected bool Equals(CharacterToken other)
        {
            return Symbol == other.Symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CharacterToken)obj);
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }
    }
}
