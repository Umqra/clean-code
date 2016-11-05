using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public delegate bool MatchStreamSymbolDelegate(char symbol, bool inText);

    public class TextTokenizer
    {
        public bool TextEnded => TextPosition == Text.Length;

        private string Text { get; set; }
        private int TextPosition { get; set; }

        private char CurrentSymbol => Text[TextPosition];
        private bool IsFirstSymbol => TextPosition == 0;
        private bool IsLastSymbol => TextPosition == Text.Length - 1;

        public TextTokenizer(string text)
        {
            Text = text;
            TextPosition = 0;
        }

        private bool LookAheadMatch(int distance, MatchStreamSymbolDelegate matchPredicate)
        {
            if (TextPosition + distance < Text.Length)
                return matchPredicate(Text[TextPosition + distance], true);
            return matchPredicate('?', false);
        }

        private bool LookBehindMatch(int distance, MatchStreamSymbolDelegate matchPredicate)
        {
            if (TextPosition - distance >= 0)
                return matchPredicate(Text[TextPosition - distance], true);
            return matchPredicate('?', false);
        }

        private bool LookAheadMatch(int distance, char symbol)
        {
            return LookAheadMatch(distance, (c, inText) => c == symbol && inText);
        }

        private bool LookBehindMatch(int distance, char symbol)
        {
            return LookBehindMatch(distance, (c, inText) => c == symbol && inText);
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

        private IToken TryParseFormatModificator(string modificator)
        {
            if (GetString(modificator.Length) != modificator)
                return null;
            if (LookAheadMatch(modificator.Length, (symbol, inText) => inText && char.IsLetterOrDigit(symbol)) &&
                LookBehindMatch(1, (symbol, inText) => inText && char.IsLetterOrDigit(symbol)))
                return null;
            return new FormatModificatorToken(TakeString(modificator.Length));
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
