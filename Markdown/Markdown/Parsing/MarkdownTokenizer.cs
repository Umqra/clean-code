using System;
using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizer : BaseTokenizer<IMdToken>
    {
        public MarkdownTokenizer(string text) : base(text)
        {
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        protected override IMdToken ParseToken()
        {
            return TryParseEscapedCharacter() ??
                   TryParseNewLineToken("  " + Environment.NewLine) ??
                   TryParseNewLineToken(Environment.NewLine + Environment.NewLine) ??
                   TryParseSemanticModificator("__", s => new MdStrongModificatorToken(s)) ??
                   TryParseSemanticModificator("_", s => new MdEmphasisModificatorToken(s)) ??
                   new MdTextToken(TakeString(1));
        }

        private IMdToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && TextPosition < Text.Length - 1)
                return new MdEscapedTextToken(TakeString(2).Substring(1));
            return null;
        }

        private IMdToken TryParseNewLineToken(string newLineToken)
        {
            if (LookAtString(newLineToken.Length) == newLineToken)
                return new MdNewLineToken(TakeString(newLineToken.Length));
            return null;
        }

        private bool CanBeSemanticModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return false;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (before.IsWhiteSpace() && after.IsWhiteSpace())
                return false;
            if (before == modificator.First() || after == modificator.Last())
                return false;
            if (before.IsLetterOrDigit() && after.IsLetterOrDigit())
                return false;
            return true;
        }

        private IMdToken TryParseSemanticModificator(string modificator, Func<string, IMdToken> modificatorConstructor)
        {
            if (!CanBeSemanticModificator(modificator))
                return null;
            return modificatorConstructor(TakeString(modificator.Length));
        }
    }
}