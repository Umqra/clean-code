using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(ATokenizer<IMdToken> tokenizer)
        {
            return new GroupNode(
                ParseNodesUntilNotNull(() => ParseParagraph(tokenizer) ?? ParseNewLine(tokenizer))
            );
        }

        public INode ParseParagraph(ATokenizer<IMdToken> tokenizer)
        {
            var children = ParseNodesUntilNotNull(() => ParseTextInParagraph(tokenizer)).ToList();
            if (children.Any())
                return new ParagraphNode(children);
            return null;
        }

        private INode ParseNewLine(ATokenizer<IMdToken> tokenizer)
        {
            var newLineToken = tokenizer.TakeTokenIfMatch<MdNewLineToken>(token => true);
            if (newLineToken != null)
                return new NewLineNode();
            return null;
        }

        private INode ParseTextInParagraph(ATokenizer<IMdToken> tokenizer)
        {
            return ParsePlainText(tokenizer) ??
                   ParseEmphasisModificator(tokenizer) ??
                   ParseStrongModificator(tokenizer);
        }

        private INode ParsePlainText(ATokenizer<IMdToken> tokenizer)
        {
            var textTokens = tokenizer.TakeTokensUntilMatch(token => token is IMdPlainTextToken);

            if (!textTokens.Any())
                return null;

            if (textTokens.TrueForAll(token => token is MdTextToken))
                return new TextNode(string.Join("", textTokens.Select(token => token.Text)));

            var children = textTokens.Select(token =>
                    token is MdEscapedTextToken
                        ? (INode)new EscapedTextNode(token.Text)
                        : (INode)new TextNode(token.Text)
            );
            return new GroupNode(children);
        }

        private INode ParseEmphasisModificator(ATokenizer<IMdToken> tokenizer)
        {
            var startToken = tokenizer.TakeTokenIfMatch<MdEmphasisModificatorToken>(t => true);

            if (startToken == null)
                return null;

            var children = ParseNodesUntilNotNull(
                () => ParsePlainText(tokenizer) ?? ParseStrongModificator(tokenizer)
            );

            var endToken = tokenizer.TakeTokenIfMatch<MdEmphasisModificatorToken>(
                token => token.Text == startToken.Text
            );

            if (endToken != null)
                return new EmphasisModificatorNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        private INode ParseStrongModificator(ATokenizer<IMdToken> tokenizer)
        {
            var startToken = tokenizer.TakeTokenIfMatch<MdStrongModificatorToken>(t => true);

            if (startToken == null)
                return null;

            var children = ParseNodesUntilNotNull(
                () => ParsePlainText(tokenizer) ?? ParseEmphasisModificator(tokenizer)
            );

            var endToken = tokenizer.TakeTokenIfMatch<MdStrongModificatorToken>(
                token => token.Text == startToken.Text
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