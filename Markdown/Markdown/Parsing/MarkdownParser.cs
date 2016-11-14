using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(ITokenizer<IMdToken> tokenizer)
        {
            return new GroupNode(
                ParseNodesUntilNotNull(() =>
                {
                    SkipNewLines(tokenizer);
                    return ParseParagraph(tokenizer);
                })
            );
        }

        public INode ParseParagraph(ITokenizer<IMdToken> tokenizer)
        {
            var children = ParseNodesUntilNotNull(() => ParseTextInParagraph(tokenizer)).ToList();
            if (children.Any())
                return new ParagraphNode(children);
            return null;
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

        private void SkipNewLines(ITokenizer<IMdToken> tokenizer)
        {
            tokenizer.TakeTokensUntilMatch(IsWhiteSpaceToken);
        }

        private INode ParseTextInParagraph(ITokenizer<IMdToken> tokenizer)
        {
            return ParsePlainText(tokenizer) ??
                   ParseModificator(tokenizer) ??
                   ParseBrokenSymbol(tokenizer);
        }

        private INode ParseBrokenSymbol(ITokenizer<IMdToken> tokenzer)
        {
            var token = tokenzer.TakeTokenIfMatch(
                t => t.Has(Md.Emphasis) || t.Has(Md.Code) || t.Has(Md.Strong)
            );
            return token == null ? null : new TextNode(token.Text);
        }

        private INode ParsePlainText(ITokenizer<IMdToken> tokenizer)
        {
            var textTokens = tokenizer.TakeTokensUntilMatch(token => token.Has(Md.PlainText) || token.Has(Md.Escaped));

            if (!textTokens.Any())
                return null;

            if (textTokens.TrueForAll(token => token.Has(Md.PlainText)))
            {
                var text = string.Join("", textTokens.Select(token => token.Text));
                return new TextNode(text);
            }

            var children = textTokens.Select(token =>
                    token.Has(Md.Escaped)
                        ? (INode)new EscapedTextNode(token.Text)
                        : (INode)new TextNode(token.Text)
            );
            return new GroupNode(children);
        }

        private INode ParseModificator(ITokenizer<IMdToken> tokenizer)
        {
            return ParseModificator(tokenizer, Md.Emphasis) ??
                   ParseModificator(tokenizer, Md.Strong) ??
                   ParseModificator(tokenizer, Md.Code);
        }

        private INode ParseModificator(ITokenizer<IMdToken> tokenizer, Md modificatorAttribute)
        {
            var startToken = tokenizer.TakeTokenIfMatch(token => token.Has(Md.Open, modificatorAttribute));

            if (startToken == null)
                return null;

            var children = ParseNodesUntilNotNull(
                () =>
                    ParsePlainText(tokenizer) ??
                    ParseModificator(tokenizer)
            );

            var endToken = tokenizer.TakeTokenIfMatch(
                token => token.Has(Md.Close, modificatorAttribute) && token.Text == startToken.Text
            );

            if (endToken != null)
                return CreateModificatorNode(modificatorAttribute, children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        // Nit: Do you need List? You can make in IEnumerable
        // and convert to List in-place.
        private List<INode> ParseNodesUntilNotNull(Func<INode> nodeFactory)
        {
            var nodes = new List<INode>();
            while (true)
            {
                var node = nodeFactory();
                if (node == null)
                    break;
                nodes.Add(node);
            }
            return nodes;
        }
    }
}