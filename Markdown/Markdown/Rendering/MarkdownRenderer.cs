using Markdown.Parsing;

namespace Markdown.Rendering
{
    public class MarkdownRenderer
    {
        public MarkdownRenderer(MarkdownParser parser, MarkdownTokenizer tokenizer, INodeRenderer nodeRenderer)
        {
            Parser = parser;
            Tokenizer = tokenizer;
            NodeRenderer = nodeRenderer;
        }

        public MarkdownParser Parser { get; set; }
        public MarkdownTokenizer Tokenizer { get; set; }
        public INodeRenderer NodeRenderer { get; set; }

        public string Render(string text)
        {
            return Parser.Parse(Tokenizer.ForText(text)).Accept(NodeRenderer);
        }
    }
}