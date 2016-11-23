using System;
using System.Collections.Generic;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.Tokenizer
{
    public static class MarkdownTokenizerExtensoins
    {
        public static MarkdownParsingResult<IMdToken> Match(this ITokenizer<IMdToken> tokenizer,
            Predicate<IMdToken> predicate)
        {
            if (tokenizer.AtEnd || !predicate(tokenizer.CurrentToken))
                return tokenizer.Fail<IMdToken>();
            return tokenizer.Advance().SuccessWith(tokenizer.CurrentToken);
        }

        public static MarkdownParsingResult<List<IMdToken>> UntilMatch(this ITokenizer<IMdToken> tokenizer,
            Predicate<IMdToken> predicate)
        {
            var tokens = new List<IMdToken>();
            while (!tokenizer.AtEnd)
            {
                var token = tokenizer.CurrentToken;
                if (!predicate(token))
                    return tokenizer.SuccessWith(tokens);
                tokenizer = tokenizer.Advance();
                tokens.Add(token);
            }
            return tokenizer.SuccessWith(tokens);
        }

        public static MarkdownParsingResult<T> Fail<T>(this ITokenizer<IMdToken> tokenizer)
        {
            return MarkdownParsingResult<T>.Fail<T>(tokenizer);
        }

        public static MarkdownParsingResult<T> SuccessWith<T>(this ITokenizer<IMdToken> tokenizer, T parsed)
        {
            return MarkdownParsingResult<T>.Success(parsed, tokenizer);
        }

        public static ITokenizer<IMdToken> UntilNotMatch(this ITokenizer<IMdToken> tokenizer,
            Predicate<IMdToken> matcher)
        {
            return new BoundedTokenizer(tokenizer, matcher);
        }

        public static ITokenizer<IMdToken> UnboundTokenizer(this ITokenizer<IMdToken> tokenizer)
        {
            if (tokenizer is BoundedTokenizer)
                return ((BoundedTokenizer)tokenizer).Tokenizer;
            return tokenizer;
        }
    }
}