using Markdown.Parsing;

namespace Markdown.Rendering
{
    public class MarkdownRenderer
    {
        public MarkdownRenderer(MarkdownParser parser, INodeRenderer nodeRenderer)
        {
            Parser = parser;
            NodeRenderer = nodeRenderer;
        }

        public MarkdownParser Parser { get; set; }
        public INodeRenderer NodeRenderer { get; set; }

        public string Render(string text)
        {
            return Parser.Parse(text).Accept(NodeRenderer);
        }
    }
}