using System;
using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizer : ATokenizer<IMdToken>
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

        private IMdToken TryParseSemanticModificator(string modificator, Func<string, IMdToken> factory)
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

            return factory(TakeString(modificator.Length));
        }
    }
}