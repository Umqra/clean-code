using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizer : ATokenizer<IToken>
    {
        public MarkdownTokenizer(string text) : base(text)
        {
        }

        private IToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && TextPosition < Text.Length - 1)
                return new EscapedCharacterToken(TakeString(2)[1]);
            return null;
        }

        //TODO: Bad function!!!
        private bool CanAttachSymbolToToken(char? symbol)
        {
            if (!symbol.HasValue)
                return false;
            return char.IsLetterOrDigit(symbol.Value) || char.IsPunctuation(symbol.Value);
        }

        private IToken TryParseFormatModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return null;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (CanAttachSymbolToToken(before) ^ CanAttachSymbolToToken(after))
            {
                if ((!before.HasValue || before.Value != modificator.First()) &&
                    (!after.HasValue || after.Value != modificator.Last()))
                    return new FormatModificatorToken(TakeString(modificator.Length));
            }
            return null;
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        protected override IToken ParseToken()
        {
            return TryParseEscapedCharacter() ??
                   TryParseFormatModificator("__") ??
                   TryParseFormatModificator("_") ??
                   new CharacterToken(TakeString(1)[0]);
        }
    }
}