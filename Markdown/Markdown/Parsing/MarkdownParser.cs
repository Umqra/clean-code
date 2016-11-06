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
            return new ParagraphNode(PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ??
                                                                  ParseBoldText(tokenizer) ??
                                                                  ParseItalicText(tokenizer)));
        }

        private INode ParsePlainText(MarkdownTokenizer tokenizer)
        {
            var textTokens =
                tokenizer.TakeTokensUntilMatch(token => token is EscapedCharacterToken || token is CharacterToken);
            if (!textTokens.Any())
                return null;
            return new TextNode(string.Join("", textTokens.Select(token => token.Text)));
        }

        private INode ParseBoldText(MarkdownTokenizer tokenizer)
        {
            var startToken = tokenizer
                .TakeTokenIfMatch(token => token.Equals(FormatModificatorToken.BoldUnderscore));
            if (startToken == null)
                return null;

            var children = PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ?? ParseItalicText(tokenizer));

            var endToken = tokenizer
                .TakeTokenIfMatch(token => token.Equals(FormatModificatorToken.BoldUnderscore));
            if (endToken != null)
                return new BoldTextNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        private INode ParseItalicText(MarkdownTokenizer tokenizer)
        {
            var startToken = tokenizer
                .TakeTokenIfMatch(token => token.Equals(FormatModificatorToken.ItalicUnderscore));
            if (startToken == null)
                return null;

            var children = PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ?? ParseBoldText(tokenizer));

            var endToken = tokenizer
                .TakeTokenIfMatch(token => token.Equals(FormatModificatorToken.ItalicUnderscore));
            if (endToken != null)
                return new ItalicTextNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        private List<INode> PraseNodesUntilNotNull(Func<INode> nodeFactory)
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