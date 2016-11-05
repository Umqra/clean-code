using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(string text)
        {
            return ParseParagraph(new TextTokenizer(text));
        }

        private INode ParseParagraph(TextTokenizer tokenizer)
        {
            return new ParagraphNode(PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ??
                                                                  ParseBoldText(tokenizer) ??
                                                                  ParseItalicText(tokenizer)));
        }

        private INode ParsePlainText(TextTokenizer tokenizer)
        {
            var currentToken = tokenizer
                .TakeNextTokenIfMatch(token => token is EscapedCharacterToken || token is CharacterToken);
            if (currentToken == null)
                return null;

            var text = new StringBuilder();
            while (currentToken != null)
            {
                text.Append(currentToken.Text);
                currentToken =
                    tokenizer.TakeNextTokenIfMatch(token => token is EscapedCharacterToken || token is CharacterToken);
            }
            return new TextNode(text.ToString());
        }

        private INode ParseBoldText(TextTokenizer tokenizer)
        {
            var startToken = tokenizer
                .TakeNextTokenIfMatch(token => token.Equals(FormatModificatorToken.BoldUnderscore));
            if (startToken == null)
                return null;

            var children = PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ?? ParseItalicText(tokenizer));

            var endToken = tokenizer
                .TakeNextTokenIfMatch(token => token.Equals(FormatModificatorToken.BoldUnderscore));
            if (endToken != null)
                return new BoldTextNode(children);
            return new GroupNode(new[] {new TextNode(startToken.Text)}.Concat(children));
        }

        private INode ParseItalicText(TextTokenizer tokenizer)
        {
            var startToken = tokenizer
                .TakeNextTokenIfMatch(token => token.Equals(FormatModificatorToken.ItalicUnderscore));
            if (startToken == null)
                return null;

            var children = PraseNodesUntilNotNull(() => ParsePlainText(tokenizer) ?? ParseBoldText(tokenizer));

            var endToken = tokenizer
                .TakeNextTokenIfMatch(token => token.Equals(FormatModificatorToken.ItalicUnderscore));
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