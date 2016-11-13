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
                   ParseEmphasisModificator(tokenizer) ??
                   ParseStrongModificator(tokenizer);
        }

        private INode ParsePlainText(ITokenizer<IMdToken> tokenizer)
        {
            var textTokens = tokenizer.TakeTokensUntilMatch(token => token.Has(Md.PlainText));

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

        private INode ParseEmphasisModificator(ITokenizer<IMdToken> tokenizer)
        {
            var startToken = tokenizer.TakeTokenIfMatch(token => token.Has(Md.Open, Md.Emphasis));

            if (startToken == null)
                return null;

            var children = ParseNodesUntilNotNull(
                () => ParsePlainText(tokenizer) ?? ParseStrongModificator(tokenizer)
            );

            var endToken = tokenizer.TakeTokenIfMatch(
                token => token.Has(Md.Close, Md.Emphasis) && token.Text == startToken.Text
            );

            if (endToken != null)
                return new EmphasisModificatorNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        private INode ParseStrongModificator(ITokenizer<IMdToken> tokenizer)
        {
            var startToken = tokenizer.TakeTokenIfMatch(token => token.Has(Md.Open, Md.Strong));

            if (startToken == null)
                return null;

            var children = ParseNodesUntilNotNull(
                () => ParsePlainText(tokenizer) ?? ParseEmphasisModificator(tokenizer)
            );

            var endToken = tokenizer.TakeTokenIfMatch(
                token => token.Has(Md.Close, Md.Strong) && token.Text == startToken.Text
            );

            if (endToken != null)
                return new StrongModificatorNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

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