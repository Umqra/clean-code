using System;
using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizer : ATokenizer<IToken>
    {
        public MarkdownTokenizer(string text) : base(text)
        {
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        protected override IToken ParseToken()
        {
            return TryParseEscapedCharacter() ??
                   TryParseNewLineToken("  " + Environment.NewLine) ??
                   TryParseNewLineToken(Environment.NewLine + Environment.NewLine) ??
                   TryParseEmphasisModificator("___") ??
                   TryParseEmphasisModificator("__") ??
                   TryParseEmphasisModificator("_") ??
                   new CharacterToken(TakeString(1)[0]);
        }

        private IToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && TextPosition < Text.Length - 1)
                return new EscapedCharacterToken(TakeString(2)[1]);
            return null;
        }

        private IToken TryParseNewLineToken(string newLineToken)
        {
            if (LookAtString(newLineToken.Length) == newLineToken)
                return new NewLineToken(TakeString(newLineToken.Length));
            return null;
        }

        private IToken TryParseEmphasisModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return null;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (before.IsWhiteSpace() && after.IsWhiteSpace())
                return null;
            if (before == modificator.First() || after == modificator.Last())
                return null;
            if (before.IsLetterOrDigit() && after.IsLetterOrDigit())
                return null;

            return new EmphasisModificatorToken(TakeString(modificator.Length));
        }
    }
}