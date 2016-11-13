﻿using System;
using System.Collections.Generic;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizer : BaseTokenizer<IMdToken>
    {
        private static readonly Dictionary<string, Md> modificatorAttribute =
            new Dictionary<string, Md>
            {
                {"__", Md.Strong},
                {"**", Md.Strong},
                {"_", Md.Emphasis},
                {"*", Md.Emphasis},
                {"`", Md.Code}
            };

        public MarkdownTokenizer(string text) : base(text)
        {
        }

        //TODO: Poor performance because of many-many CharacterToken objects
        protected override IMdToken ParseToken()
        {
            return TryParseEscapedCharacter() ??
                   TryParseNewLineToken() ??
                   TryParseModificator("__", "**", "_", "*", "`") ??
                   new MdToken(TakeString(1)).With(Md.PlainText);
        }

        private IMdToken TryParseNewLineToken()
        {
            return TryParseNewLineToken("  " + Environment.NewLine) ??
                   TryParseNewLineToken(Environment.NewLine + Environment.NewLine);
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
                return new MdToken(TakeString(modificator.Length)).With(Md.Open);
            if ((before.IsWhiteSpace() || !before.HasValue) && after.HasValue && !after.IsWhiteSpace())
                return new MdToken(TakeString(modificator.Length)).With(Md.Open);
            return null;
        }

        private IMdToken TryParseCloseModificator(string modificator)
        {
            if (LookAtString(modificator.Length) != modificator)
                return null;
            var before = LookBehind(1);
            var after = LookAhead(modificator.Length);

            if (before.IsLetterOrDigit() && after.IsPunctuation())
                return new MdToken(TakeString(modificator.Length)).With(Md.Close);
            if (before.HasValue && !before.IsWhiteSpace() && (after.IsWhiteSpace() || !after.HasValue))
                return new MdToken(TakeString(modificator.Length)).With(Md.Close);
            return null;
        }

        private IMdToken TryParseEscapedCharacter()
        {
            if (CurrentSymbol == '\\' && TextPosition < Text.Length - 1)
                return new MdToken(TakeString(2).Substring(1)).With(Md.Escaped);
            return null;
        }

        private IMdToken TryParseNewLineToken(string newLineToken)
        {
            if (LookAtString(newLineToken.Length) == newLineToken)
                return new MdToken(TakeString(newLineToken.Length)).With(Md.NewLine);
            return null;
        }
    }
}