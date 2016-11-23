using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.Tokenizer
{
    public sealed class MarkdownTokenizer : BaseTokenizer<IMdToken>
    {
        private const int HeaderTokenMaxSize = 6;

        private static readonly Dictionary<string, Md> modificatorAttribute =
            new Dictionary<string, Md>
            {
                {"__", Md.Strong},
                {"**", Md.Strong},
                {"_", Md.Emphasis},
                {"*", Md.Emphasis},
                {"`", Md.Code}
            };

        public override IMdToken CurrentToken { get; }

        public MarkdownTokenizer(string text) : this(text, 0)
        {
        }

        private MarkdownTokenizer(string text, int textPosition) : base(text, textPosition)
        {
            CurrentToken = AtEnd ? null : ParseToken();
        }

        public override ITokenizer<IMdToken> Advance()
        {
            if (AtEnd)
                return this;
            return new MarkdownTokenizer(Text, TextPosition + CurrentToken.UnderlyingText.Length);
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        private IMdToken ParseToken()
        {
            return TryParseHeaderToken() ??
                   TryParseEscapedCharacter() ??
                   TryParseBreakParagraphToken() ??
                   TryParseNewLineToken() ?? 
                   TryParseModificator("__", "**", "_", "*", "`") ??
                   TryParseLinkTokens() ??
                   new MdToken(LookAtString(1)).With(Md.PlainText);
        }

        private IMdToken TryParseNewLineToken()
        {
            return (TryParseSimpleToken("\n") ?? TryParseSimpleToken(Environment.NewLine))?.With(Md.NewLine);
        }

        private IMdToken TryParseHeaderToken()
        {
            var previous = LookBehind(1);
            if (previous.HasValue && previous.Value != '\n')
                return null;
            var prefix = LookAtString(HeaderTokenMaxSize + 1);
            var leadHashes = new string(prefix.TakeWhile(c => c == '#').ToArray());
            if (leadHashes.Length == 0 || leadHashes.Length == prefix.Length)
                return null;
            int skippedSpaces = 0;
            while (LookAhead(leadHashes.Length + skippedSpaces) == ' ')
                skippedSpaces++;
            return new MdToken(leadHashes, leadHashes + new string(' ', skippedSpaces)).With(Md.Header);
        }

        private IMdToken TryParseLinkTokens()
        {
            return (TryParseToken("[", Md.Open) ?? TryParseToken("]", Md.Close))?.With(Md.LinkText) ??
                   (TryParseToken("(", Md.Open) ?? TryParseToken(")", Md.Close))?.With(Md.LinkReference);
        }

        private IMdToken TryParseToken(string token, params Md[] attributes)
        {
            if (LookAtString(token.Length) == token)
                return new MdToken(token).With(attributes);
            return null;
        }

        private IMdToken TryParseBreakParagraphToken()
        {
            return (TryParseSimpleToken("  " + Environment.NewLine) ??
                    TryParseSimpleToken(Environment.NewLine + Environment.NewLine))
                    ?.With(Md.Break);
        }

        private IMdToken TryParseModificator(params string[] modificators)
        {
            IMdToken token = null;
            foreach (var modificator in modificators)
                token = token ?? TryParseOpenModificator(modificator) ?? TryParseCloseModificator(modificator);
            return token?.With(modificatorAttribute[token.Text]);
        }

        private IMdToken TryParseOpenModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return null;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (before.IsPunctuation() && after.IsLetterOrDigit())
                return new MdToken(modificator).With(Md.Open);
            if ((before.IsWhiteSpace() || !before.HasValue) && after.HasValue && !after.IsWhiteSpace())
                return new MdToken(modificator).With(Md.Open);
            return null;
        }

        private IMdToken TryParseCloseModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return null;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (before.IsLetterOrDigit() && after.IsPunctuation())
                return new MdToken(modificator).With(Md.Close);
            if (before.HasValue && !before.IsWhiteSpace() && (after.IsWhiteSpace() || !after.HasValue))
                return new MdToken(modificator).With(Md.Close);
            return null;
        }

        private IMdToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && TextPosition < Text.Length - 1)
                return new MdToken(LookAtString(2).Substring(1), LookAtString(2)).With(Md.Escaped);
            return null;
        }

        private IMdToken TryParseSimpleToken(string token)
        {
            if (LookAtString(token.Length) == token)
                return new MdToken(token);
            return null;
        }
    }
}