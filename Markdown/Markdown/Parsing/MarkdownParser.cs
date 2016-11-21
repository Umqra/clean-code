using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public MarkdownParsingResult<INode> Parse(ITokenizer<IMdToken> tokenizer)
        {
            var children = ParseNodesUntilMatch(tokenizer,
                t =>
                {
                    var skipped = SkipWhiteSpaces(t.UnboundTokenizer());
                    return skipped.IfSuccess(paragraphTokenizer => ParseParagraph(
                        paragraphTokenizer.UntilNotMatch(token => token.Has(Md.NewLine))
                    ));
                }
            );
            return children.Remainder.SuccessWith<INode>(new GroupNode(children.Parsed));
        }

        public MarkdownParsingResult<INode> ParseParagraph(ITokenizer<IMdToken> tokenizer)
        {
            var children = ParseNodesUntilMatch(tokenizer, ParseParagraphContent);
            if (children.Parsed.Any())
                return children.Remainder.SuccessWith<INode>(new ParagraphNode(children.Parsed));
            return tokenizer.Fail<INode>();
        }

        private INode CreateModificatorNode(Md modificatorAttribute, IEnumerable<INode> nodes)
        {
            switch (modificatorAttribute)
            {
                case Md.Emphasis:
                    return new EmphasisModificatorNode(nodes);
                case Md.Strong:
                    return new StrongModificatorNode(nodes);
                case Md.Code:
                    return new CodeModificatorNode(nodes);
                default:
                    throw new ArgumentException($"Unknown modificator attribute: {modificatorAttribute}");
            }
        }

        private bool IsWhiteSpaceToken(IMdToken token)
        {
            return token.Text.All(char.IsWhiteSpace);
        }

        private MarkdownParsingResult<List<IMdToken>> SkipWhiteSpaces(ITokenizer<IMdToken> tokenizer)
        {
            return tokenizer.UntilMatch(IsWhiteSpaceToken);
        }

        private MarkdownParsingResult<INode> ParseFormatModificator(ITokenizer<IMdToken> tokenizer)
        {
            return ParseFormatModificator(tokenizer, Md.Emphasis)
                .IfFail(t => ParseFormatModificator(t, Md.Strong));
        }

        private MarkdownParsingResult<INode> ParseBrokenSymbol(ITokenizer<IMdToken> tokenizer)
        {
            if (tokenizer.AtEnd)
                return tokenizer.Fail<INode>();
            return tokenizer.Advance().SuccessWith<INode>(new TextNode(tokenizer.CurrentToken.Text));
        }

        private MarkdownParsingResult<INode> ParseParagraphContent(ITokenizer<IMdToken> tokenizer)
        {
            return ParseText(tokenizer)
                .IfFail(ParseCodeModificator)
                .IfFail(ParseFormatModificator)
                .IfFail(ParseLink)
                .IfFail(ParseBrokenSymbol);
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

        private MarkdownParsingResult<INode> ParseText(ITokenizer<IMdToken> tokenizer)
        {
            var parsingResult = ParseNodesUntilMatch(tokenizer, t => ParsePlainText(t).IfFail(ParseEscaped));
            var nodes = parsingResult.Parsed;

            if (!nodes.Any())
                return tokenizer.Fail<INode>();

            // No need to create extra nodes if we can
            INode result = nodes.Count == 1 ? nodes[0] : new GroupNode(nodes);
            return parsingResult.Remainder.SuccessWith(result);
        }

        private MarkdownParsingResult<INode> ParseCodeModificator(ITokenizer<IMdToken> tokenizer)
        {
            var boundedTokenizer = tokenizer.UntilNotMatch(token => token.Has(Md.Close, Md.Code));

            var open = boundedTokenizer.Match(token => token.Has(Md.Open, Md.Code));
            var children = open.IfSuccess(childrenTokenizer => ParseNodesUntilMatch(childrenTokenizer, ParseBrokenSymbol));

            var close = children.IfSuccess(t => t.UnboundTokenizer()
                .Match(token => token.Has(Md.Close, Md.Code)));
            if (close.Succeed)
            {
                return close.Remainder.SuccessWith<INode>(new CodeModificatorNode(children.Parsed));
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseFormatModificator(ITokenizer<IMdToken> tokenizer, Md modificatorAttribute)
        {
            var boundedTokenizer = tokenizer.UntilNotMatch(token => token.Has(Md.Close, modificatorAttribute));

            var open = boundedTokenizer.Match(token => token.Has(Md.Open, modificatorAttribute));

            var children = open.IfSuccess(
                childrenTokenizer => ParseNodesUntilMatch(childrenTokenizer,
                    t => ParseText(t)
                        .IfFail(ParseFormatModificator)
                        .IfFail(ParseBrokenSymbol))
            );

            var close = children.IfSuccess(t => t.UnboundTokenizer()
                    .Match(token => token.Has(Md.Close, modificatorAttribute))
            );

            if (close.Succeed && close.Parsed.Text == open.Parsed.Text)
            {
                var node = CreateModificatorNode(modificatorAttribute, children.Parsed);
                return close.Remainder.SuccessWith(node);
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseLink(ITokenizer<IMdToken> tokenizer)
        {
            var openText = tokenizer.Match(token => token.Has(Md.LinkText, Md.Open));
            var text = openText.IfSuccess(ParseText);
            var closeText = text.IfSuccess(t => t.Match(token => token.Has(Md.LinkText, Md.Close)));

            var openLink = closeText
                .IfSuccess(SkipWhiteSpaces)
                .IfSuccess(t => t.Match(token => token.Has(Md.LinkReference, Md.Open)));
            var link = openLink.IfSuccess(ParsePlainText);
            var closeLink = link.IfSuccess(t => t.Match(token => token.Has(Md.LinkReference, Md.Close)));

            if (closeLink.Succeed)
            {
                var linkText = ((TextNode)link.Parsed).Text;
                return closeLink.Remainder.SuccessWith<INode>(new LinkNode(linkText, text.Parsed));
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<List<T>> ParseNodesUntilMatch<T>(ITokenizer<IMdToken> tokenizer,
            Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> matcher)
        {
            var nodes = new List<T>();
            while (true)
            {
                var result = matcher(tokenizer);
                if (!result.Succeed)
                    return result.Remainder.SuccessWith(nodes);
                tokenizer = result.Remainder;
                nodes.Add(result.Parsed);
            }
        }
    }
}