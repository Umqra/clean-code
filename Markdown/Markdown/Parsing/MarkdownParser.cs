using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(ATokenizer<IToken> tokenizer)
        {
            return new GroupNode(
                ParseNodesUntilNotNull(() => ParseParagraph(tokenizer) ?? ParseNewLine(tokenizer))
            );
        }

        public INode ParseParagraph(ATokenizer<IToken> tokenizer)
        {
            var children = ParseNodesUntilNotNull(() => ParseTextInParagraph(tokenizer)).ToList();
            if (children.Any())
                return new ParagraphNode(children);
            return null;
        }

        private INode ParseNewLine(ATokenizer<IToken> tokenizer)
        {
            var newLineToken = tokenizer.TakeTokenIfMatch<NewLineToken>(token => true);
            if (newLineToken != null)
                return new NewLineNode();
            return null;
        }

        private INode ParseTextInParagraph(ATokenizer<IToken> tokenizer)
        {
            return ParsePlainText(tokenizer) ??
                   ParseEmphasisText(tokenizer, EmphasisExtensions.GetAllEmphasisValues());
        }

        private INode ParsePlainText(ATokenizer<IToken> tokenizer)
        {
            var textTokens = tokenizer.TakeTokensUntilMatch(token => token is IPlainTextToken);

            if (!textTokens.Any())
                return null;

            if (textTokens.TrueForAll(token => token is CharacterToken))
                return new TextNode(string.Join("", textTokens.Select(token => token.Text)));

            var children = textTokens.Select(token =>
                    token is EscapedCharacterToken
                        ? (INode)new EscapedTextNode(token.Text)
                        : (INode)new TextNode(token.Text)
            );
            return new GroupNode(children);
        }

        private INode ParseEmphasisText(ATokenizer<IToken> tokenizer, EmphasisStrength[] parsingStrengths)
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