using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.ParsingElements;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public MarkdownParsingResult<INode> Parse(ITokenizer<IMdToken> tokenizer)
        {
            var skip = MdTokenizerAction.SkipTokens(Md.NewLine, Md.Break);
            var header = MdParserAction.Parse(ParseHeader);
            var paragraph = MdTokenizerAction
                .BoundTokenizer(token => token.HasAny(Md.Header, Md.Break))
                .Then(MdParserAction.Parse(ParseParagraph))
                .Then(MdTokenizerAction.UnboundTokenizer());

            var parser = skip.Then(header.Or(paragraph)).UntilSucceed();
            tokenizer = parser.Do(tokenizer);
            return tokenizer.SuccessWith<INode>(new GroupNode(parser.Parsed));
        }

        public MarkdownParsingResult<INode> ParseParagraph(ITokenizer<IMdToken> tokenizer)
        {
            var content = MdParserAction.Parse(ParseFormattedText).UntilSucceed();
            tokenizer = content.Do(tokenizer);

            if (content.Parsed.Any())
                return tokenizer.SuccessWith<INode>(new ParagraphNode(content.Parsed));
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseHeader(ITokenizer<IMdToken> tokenizer)
        {
            var headerToken = MdParserAction.ParseToken(Md.Header);
            var header = MdTokenizerAction
                .BoundTokenizer(token => token.HasAny(Md.NewLine, Md.Break))
                .Then(MdTokenizerAction.Parse(SkipWhiteSpaces))
                .Then(MdParserAction.Parse(ParseFormattedText).UntilSucceed())
                .Then(MdTokenizerAction.UnboundTokenizer());
            var parser = headerToken.Then(header);
            var advancedTokenizer = parser.Do(tokenizer);
            
            if (parser.Succeed)
            {
                return advancedTokenizer.SuccessWith<INode>(
                    new HeaderNode(headerToken.Parsed.Text.Length, header.Parsed)
                );
            }
            return tokenizer.Fail<INode>();
        }

        private INode CreateModifierNode(Md modifierAttribute, IEnumerable<INode> nodes)
        {
            switch (modifierAttribute)
            {
                case Md.Emphasis:
                    return new EmphasisModifierNode(nodes);
                case Md.Strong:
                    return new StrongModifierNode(nodes);
                case Md.Code:
                    return new CodeModifierNode(nodes);
                default:
                    throw new ArgumentException($"Unknown modifier attribute: {modifierAttribute}");
            }
        }

        private bool IsWhiteSpaceToken(IMdToken token)
        {
            return token.Text.All(char.IsWhiteSpace);
        }

        private ITokenizer<IMdToken> SkipWhiteSpaces(ITokenizer<IMdToken> tokenizer)
        {
            return tokenizer.UntilMatch(IsWhiteSpaceToken).Remainder;
        }

        private MarkdownParsingResult<List<IMdToken>> SkipWhiteSpacesOld(ITokenizer<IMdToken> tokenizer)
        {
            return tokenizer.UntilMatch(IsWhiteSpaceToken);
        }

        private MarkdownParsingResult<INode> ParseFormatModifier(ITokenizer<IMdToken> tokenizer)
        {
            return ParseFormatModifier(tokenizer, Md.Emphasis)
                .IfFail(t => ParseFormatModifier(t, Md.Strong));
        }

        private MarkdownParsingResult<INode> ParseAnyTokenAsText(ITokenizer<IMdToken> tokenizer)
        {
            if (tokenizer.AtEnd)
                return tokenizer.Fail<INode>();
            return tokenizer.Advance().SuccessWith<INode>(new TextNode(tokenizer.CurrentToken.Text));
        }

        private MarkdownParsingResult<INode> ParseAnyTokenAsEscaped(ITokenizer<IMdToken> tokenizer)
        {
            if (tokenizer.AtEnd)
                return tokenizer.Fail<INode>();
            var underlyingText = tokenizer.CurrentToken.UnderlyingText;
            return tokenizer.Advance().SuccessWith<INode>(new EscapedTextNode(underlyingText));
        }

        private MarkdownParsingResult<INode> ParseFormattedText(ITokenizer<IMdToken> tokenizer)
        {
            return ParseTextWithEscaped(tokenizer)
                .IfFail(ParseCode)
                .IfFail(ParseFormatModifier)
                .IfFail(ParseLink)
                .IfFail(ParseAnyTokenAsText);
        }

        private MarkdownParsingResult<INode> ParseCode(ITokenizer<IMdToken> tokenizer)
        {
            return ParseCodeInBackticks(tokenizer).IfFail(ParseCodeIndented);
        }

        private MarkdownParsingResult<INode> ParseCodeIndented(ITokenizer<IMdToken> tokenizer)
        {
            var indent = MdParserAction.ParseToken(Md.Indent);
            var codeLine = MdTokenizerAction
                .BoundTokenizer(token => token.Has(Md.NewLine))
                .Then(MdParserAction.Parse(ParseAnyTokenAsEscaped).UntilSucceed())
                .Then(MdTokenizerAction.UnboundTokenizer());
            var newLine = MdParserAction.ParseToken(Md.NewLine);

            var parser = indent.Then(codeLine).Then(newLine);
            var lineNodes = new List<INode>();
            while (true)
            {
                tokenizer = parser.Do(tokenizer);
                if (codeLine.Succeed)
                    lineNodes.AddRange(codeLine.Parsed);
                if (newLine.Succeed)
                    lineNodes.Add(new EscapedTextNode(newLine.Parsed.Text));
                if (!parser.Succeed)
                {
                    if (lineNodes.Any())
                        return tokenizer.SuccessWith<INode>(new CodeModifierNode(lineNodes));
                    return tokenizer.Fail<INode>();
                }
            }
        }

        private MarkdownParsingResult<INode> ParsePlainText(ITokenizer<IMdToken> tokenizer)
        {
            var tokens = tokenizer.UntilMatch(token => token.Has(Md.PlainText));
            var text = string.Join("", tokens.Parsed.Select(t => t.Text));
            if (tokens.Parsed.Any())
                return tokens.Remainder.SuccessWith<INode>(new TextNode(text));
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseEscaped(ITokenizer<IMdToken> tokenizer)
        {
            var tokens = tokenizer.Match(token => token.Has(Md.Escaped));
            if (tokens.Succeed)
                return tokens.Remainder.SuccessWith<INode>(new EscapedTextNode(tokens.Parsed.Text));
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseTextWithEscaped(ITokenizer<IMdToken> tokenizer)
        {
            var plainText = MdParserAction.Parse(ParsePlainText);
            var escapedText = MdParserAction.Parse(ParseEscaped);
            var parser = plainText.Or(escapedText).UntilSucceed();
            var advancedTokenizer = parser.Do(tokenizer);
            var nodes = parser.Parsed;

            if (!nodes.Any())
                return tokenizer.Fail<INode>();

            // No need to create extra nodes if we can
            INode result = nodes.Count == 1 ? nodes[0] : new GroupNode(nodes);
            return advancedTokenizer.SuccessWith(result);
        }

        private MarkdownParsingResult<INode> ParseCodeInBackticks(ITokenizer<IMdToken> tokenizer)
        {
            var open = MdParserAction.ParseToken(Md.Open, Md.Code);
            var code = MdTokenizerAction
                .BoundTokenizer(token => token.Has(Md.Close, Md.Code))
                .Then(MdParserAction.Parse(ParseAnyTokenAsEscaped).UntilSucceed())
                .Then(MdTokenizerAction.UnboundTokenizer());
            var close = MdParserAction.ParseToken(Md.Close, Md.Code);
            var parser = open.Then(code).Then(close);
            var advancedTokenizer = parser.Do(tokenizer);

            if (parser.Succeed)
            {
                return advancedTokenizer.SuccessWith<INode>(new CodeModifierNode(code.Parsed));
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseFormatModifier(ITokenizer<IMdToken> tokenizer,
            Md modifierAttribute)
        {
            var open = MdParserAction.ParseToken(Md.Open, modifierAttribute);
            var text = MdTokenizerAction
                .BoundTokenizer(token => token.Has(Md.Close, modifierAttribute))
                .Then(MdParserAction
                    .Parse(ParseTextWithEscaped)
                    .Or(MdParserAction.Parse(ParseFormatModifier))
                    .Or(MdParserAction.Parse(ParseAnyTokenAsText))
                    .UntilSucceed())
                .Then(MdTokenizerAction.UnboundTokenizer());
            var close = MdParserAction.ParseToken(Md.Close, modifierAttribute);
            var parser = open.Then(text).Then(close);
            var advancedTokenizer = parser.Do(tokenizer);

            if (parser.Succeed && close.Parsed.Text == open.Parsed.Text)
            {
                var node = CreateModifierNode(modifierAttribute, text.Parsed);
                return advancedTokenizer.SuccessWith(node);
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseLink(ITokenizer<IMdToken> tokenizer)
        {
            var openText = MdParserAction.ParseToken(Md.LinkText, Md.Open);
            var text = MdParserAction.Parse(ParseTextWithEscaped);
            var closeText = MdParserAction.ParseToken(Md.LinkText, Md.Close);

            var skip = MdTokenizerAction.Parse(SkipWhiteSpaces);

            var openLink = MdParserAction.ParseToken(Md.LinkReference, Md.Open);
            var link = MdParserAction.Parse(ParsePlainText);
            var closeLink = MdParserAction.ParseToken(Md.LinkReference, Md.Close);
            var parser = 
                openText.Then(text).Then(closeText)
                .Then(skip)
                .Then(openLink).Then(link).Then(closeLink);
            var advancedTokenizer = parser.Do(tokenizer);

            if (parser.Succeed)
            {
                var linkText = ((TextNode)link.Parsed).Text;
                return advancedTokenizer.SuccessWith<INode>(new LinkNode(linkText, text.Parsed));
            }
            return tokenizer.Fail<INode>();
        }
    }
}