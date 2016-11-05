using System;
using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public INode Parse(string text)
        {
            return ParseParagraph(new TextTokenizer(text));
        }

        private INode ParseParagraph(TextTokenizer text)
        {
            return new ParagraphNode(new INode[] {});
        }
    }
}