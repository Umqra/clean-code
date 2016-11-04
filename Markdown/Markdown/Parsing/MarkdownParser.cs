using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public ParagraphNode Parse(string text)
        {
            return new ParagraphNode(new [] {new TextNode(text)});
        }
    }
}