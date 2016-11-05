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
        private string Text { get; set; }

        private int TextPosition { get; set; }

        private char CurrentSymbol => Text[TextPosition];
        private bool IsFirstSymbol => TextPosition == 0;
        private bool IsLastSymbol => TextPosition == Text.Length - 1;
        private bool TextEnded => TextPosition == Text.Length;

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

        private string Take(int count)
        {
            return Text.Substring(TextPosition, Math.Min(Text.Length - TextPosition, count));
        }

        private string TakeAndMovePosition(int count)
        {
            var result = Take(count);
            TextPosition += result.Length;
            return result;
        }

        private IToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && !IsLastSymbol)
                return new EscapedCharacterToken(TakeAndMovePosition(2)[1]);
            return null;
        }

        private IToken TryParseOpenModificator(string modificator)
        {
            if (Take(modificator.Length) != modificator)
                return null;
            if (LookAheadMatch(modificator.Length, (symbol, text) => text && !char.IsWhiteSpace(symbol)) &&
                LookBehindMatch(1, (symbol, text) => !text || char.IsWhiteSpace(symbol)))
                return new OpenModificatorToken(TakeAndMovePosition(modificator.Length));
            return null;
        }

        private IToken TryParseCloseModificator(string modificator)
        {
            if (Take(modificator.Length) != modificator)
                return null;
            if (LookAheadMatch(modificator.Length, (symbol, text) => !text || char.IsWhiteSpace(symbol)) && 
                LookBehindMatch(1, (symbol, text) => text && !char.IsWhiteSpace(symbol)))
                return new CloseModificatorToken(TakeAndMovePosition(modificator.Length));
            return null;
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        public IToken GetNextToken()
        {
            if (TextEnded)
                return null;

            return TryParseEscapedCharacter() ??
                   TryParseOpenModificator("__") ??
                   TryParseOpenModificator("_") ??
                   TryParseCloseModificator("__") ??
                   TryParseCloseModificator("_") ??
                   new CharacterToken(TakeAndMovePosition(1)[0]);
        }
    }
}
