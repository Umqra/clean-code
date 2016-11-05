using System;
using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class TextTokenizer
    {
        public TextTokenizer(string text)
        {
            Text = text;
            TextPosition = 0;
        }

        public bool TextEnded => TextPosition == Text.Length;

        private string Text { get; }
        private int TextPosition { get; set; }

        private char CurrentSymbol => Text[TextPosition];
        private bool IsLastSymbol => TextPosition == Text.Length - 1;

        private char? LookAhead(int distance)
        {
            if (TextPosition + distance < Text.Length)
                return Text[TextPosition + distance];
            return null;
        }

        private char? LookBehind(int distance)
        {
            if (TextPosition - distance >= 0)
                return Text[TextPosition - distance];
            return null;
        }

        public string GetString(int length)
        {
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, length));
        }

        public string TakeString(int length)
        {
            var result = GetString(length);
            TextPosition += result.Length;
            return result;
        }

        private IToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && !IsLastSymbol)
                return new EscapedCharacterToken(TakeString(2)[1]);
            return null;
        }

        private bool CanAttachSymbolToToken(char? symbol)
        {
            if (!symbol.HasValue)
                return false;
            return char.IsLetterOrDigit(symbol.Value);
        }

        private IToken TryParseFormatModificator(string modificator)
        {
            if (GetString(modificator.Length) != modificator)
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
        private IToken ParseToken()
        {
            return TryParseEscapedCharacter() ??
                   TryParseFormatModificator("__") ??
                   TryParseFormatModificator("_") ??
                   new CharacterToken(TakeString(1)[0]);
        }

        public IToken GetNextToken()
        {
            var positionDump = TextPosition;
            var token = TakeNextToken();
            TextPosition = positionDump;

            return token;
        }

        public IToken TakeNextToken()
        {
            if (TextEnded)
                return null;
            return ParseToken();
        }

        public IToken TakeNextTokenIfMatch(Predicate<IToken> matchPredicate)
        {
            if (TextEnded)
                return null;
            var positionDump = TextPosition;
            var token = TakeNextToken();
            if (token != null && matchPredicate(token))
                return token;
            TextPosition = positionDump;
            return null;
        }
    }
}