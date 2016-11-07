using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(string text)
        {
            return ParseParagraph(new MarkdownTokenizer(text));
        }

        private INode ParseParagraph(MarkdownTokenizer tokenizer)
        {
            return new ParagraphNode(ParseNodesUntilNotNull(() => ParseTextInParagraph(tokenizer)));
        }

        private INode ParseTextInParagraph(MarkdownTokenizer tokenizer)
        {
            return ParsePlainText(tokenizer) ??
                   ParseEmphasisText(tokenizer, EmphasisExtensions.GetAllEmphasisValues());
        }

        private INode ParsePlainText(MarkdownTokenizer tokenizer)
        {
            var textTokens = tokenizer.TakeTokensUntilMatch(
                token => token is EscapedCharacterToken || token is CharacterToken
            );

            if (!textTokens.Any())
                return null;
            return new TextNode(string.Join("", textTokens.Select(token => token.Text)));
        }

        private INode ParseEmphasisText(MarkdownTokenizer tokenizer, EmphasisStrength[] parsingStrengths)
        {
            var startToken = tokenizer.TakeTokenIfMatch<EmphasisModificatorToken>(
                token => parsingStrengths.Contains(token.EmphasisStrength)
            );

            if (startToken == null)
                return null;

            var allEmphasisExceptStart = startToken.EmphasisStrength.ExcludeFromEmphasisValues();
            var children = ParseNodesUntilNotNull(
                () =>
                    ParsePlainText(tokenizer) ??
                    ParseEmphasisText(tokenizer, allEmphasisExceptStart)
            );

            var endToken = tokenizer.TakeTokenIfMatch<EmphasisModificatorToken>(
                token => token.Text == startToken.Text
            );

            if (endToken != null)
                return new EmphasisTextNode(startToken.EmphasisStrength, children);
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