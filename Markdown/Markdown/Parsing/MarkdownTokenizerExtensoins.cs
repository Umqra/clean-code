using System;
using System.Collections.Generic;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public static class MarkdownTokenizerExtensoins
    {
        public static MarkdownParsingResult<IMdToken> Match(this ITokenizer<IMdToken> tokenizer,
            Predicate<IMdToken> predicate)
        {
            if (tokenizer.AtEnd || !predicate(tokenizer.CurrentToken))
                return tokenizer.Fail<IMdToken>();
            return tokenizer.Advance().Success(tokenizer.CurrentToken);
        }

        public static MarkdownParsingResult<List<IMdToken>> UntilMatch(this ITokenizer<IMdToken> tokenizer,
            Predicate<IMdToken> predicate)
        {
            var tokens = new List<IMdToken>();
            while (!tokenizer.AtEnd)
            {
                var token = tokenizer.CurrentToken;
                if (!predicate(token))
                    return tokenizer.Success(tokens);
                tokenizer = tokenizer.Advance();
                tokens.Add(token);
            }
            return tokenizer.Success(tokens);
        }

        public static MarkdownParsingResult<T> Fail<T>(this ITokenizer<IMdToken> tokenizer)
        {
            return MarkdownParsingResult<T>.Fail<T>(tokenizer);
        }

        public static MarkdownParsingResult<T> Success<T>(this ITokenizer<IMdToken> tokenizer, T parsed)
        {
            return MarkdownParsingResult<T>.Success(parsed, tokenizer);
        }
    }
}