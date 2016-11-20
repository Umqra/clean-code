using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public MarkdownParsingResult<INode> Parse(ITokenizer<IMdToken> tokenizer)
        {
            var parsed = ParseNodesUntilNotNull(tokenizer, t => SkipNewLines(t).IfSuccess(ParseParagraph));
            return parsed.Remainder.Success<INode>(new GroupNode(parsed.Parsed));
        }

        public MarkdownParsingResult<INode> ParseParagraph(ITokenizer<IMdToken> tokenizer)
        {
            var childrenParsed = ParseNodesUntilNotNull(tokenizer, ParseParagraphContent);
            if (childrenParsed.Parsed.Any())
                return childrenParsed.Remainder.Success<INode>(new ParagraphNode(childrenParsed.Parsed));
            return tokenizer.Fail<INode>();
        }

        public INode CreateModificatorNode(Md modificatorAttribute, IEnumerable<INode> nodes)
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
            return token.Has(Md.NewLine) || token.Text.All(char.IsWhiteSpace);
        }

        private MarkdownParsingResult<List<IMdToken>> SkipNewLines(ITokenizer<IMdToken> tokenizer)
        {
            return tokenizer.UntilMatch(IsWhiteSpaceToken);
        }

        private MarkdownParsingResult<INode> ParseParagraphContent(ITokenizer<IMdToken> tokenizer)
        {
            return ParseText(tokenizer)
                .IfFail(ParseModificator)
                .IfFail(ParseBrokenSymbol);
        }

        private MarkdownParsingResult<INode> ParseBrokenSymbol(ITokenizer<IMdToken> tokenzer)
        {
            var brokenParsed = tokenzer.Match(
                t => t.Has(Md.Emphasis) || t.Has(Md.Code) || t.Has(Md.Strong)
            );
            if (brokenParsed.Succeed)
            {
                var reason = $"Unexpected token {brokenParsed.Parsed.Text}. May be you need to escape it.";
                INode node = new BrokenTextNode(brokenParsed.Parsed.Text, reason);
                return brokenParsed.Remainder.Success(node);
            }
            return brokenParsed.Remainder.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParsePlainText(ITokenizer<IMdToken> tokenizer)
        {
            var tokensParsed = tokenizer.UntilMatch(token => token.Has(Md.PlainText));
            var tokens = tokensParsed.Parsed;
            var text = string.Join("", tokens.Select(t => t.Text));
            if (tokens.Any())
                return tokensParsed.Remainder.Success<INode>(new TextNode(text));
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseEscaped(ITokenizer<IMdToken> tokenizer)
        {
            var tokenParsed = tokenizer.Match(token => token.Has(Md.Escaped));
            if (tokenParsed.Succeed)
                return tokenParsed.Remainder.Success<INode>(new EscapedTextNode(tokenParsed.Parsed.Text));
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<INode> ParseText(ITokenizer<IMdToken> tokenizer)
        {
            var parsedNodes = ParseNodesUntilNotNull(tokenizer, t => ParsePlainText(t).IfFail(ParseEscaped));
            var nodes = parsedNodes.Parsed;

            if (!nodes.Any())
                return tokenizer.Fail<INode>();
            INode result = nodes.Count == 1 ? nodes[0] : new GroupNode(nodes);
            return parsedNodes.Remainder.Success(result);
        }

        private MarkdownParsingResult<INode> ParseModificator(ITokenizer<IMdToken> tokenizer)
        {
            return ParseModificator(tokenizer, Md.Emphasis)
                .IfFail(t => ParseModificator(t, Md.Strong))
                .IfFail(t => ParseModificator(t, Md.Code));
        }

        private MarkdownParsingResult<INode> ParseModificator(ITokenizer<IMdToken> tokenizer, Md modificatorAttribute)
        {
            var startParsed = tokenizer.Match(token => token.Has(Md.Open, modificatorAttribute));

            var childrenParsed = startParsed.IfSuccess(
                currentT => ParseNodesUntilNotNull(currentT, t => ParseText(t).IfFail(ParseModificator))
            );

            var endParsed = childrenParsed.IfSuccess(t => t.Match(token => token.Has(Md.Close, modificatorAttribute)));

            if (endParsed.Succeed && endParsed.Parsed.Text == startParsed.Parsed.Text)
            {
                var node = CreateModificatorNode(modificatorAttribute, childrenParsed.Parsed);
                return endParsed.Remainder.Success(node);
            }
            return tokenizer.Fail<INode>();
        }

        private MarkdownParsingResult<List<T>> ParseNodesUntilNotNull<T>(ITokenizer<IMdToken> tokenizer,
            Func<ITokenizer<IMdToken>, MarkdownParsingResult<T>> nodeFactory)
        {
            var nodes = new List<T>();
            while (true)
            {
                var result = nodeFactory(tokenizer);
                if (!result.Succeed)
                    return result.Remainder.Success(nodes);
                tokenizer = result.Remainder;
                nodes.Add(result.Parsed);
            }
        }
    }
}