﻿using Markdown.Parsing.Tokenizer.Input;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing.Tokenizer
{
    public class MarkdownTokenizerFactory : ITokenizerFactory<IMdToken>
    {
        public ITokenizer<IMdToken> CreateTokenizer(string text)
        {
            return new MarkdownTokenizer(new StringInput(text));
        }
    }
}