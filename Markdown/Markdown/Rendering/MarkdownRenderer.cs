using Markdown.Parsing;

namespace Markdown.Rendering
{
    class MarkdownRenderer
    {
        public MarkdownParser Parser { get; set; }
        public INodeRenderer NodeRenderer { get; set; } 
        public MarkdownRenderer(MarkdownParser parser, INodeRenderer nodeRenderer)
        {
            Parser = parser;
            NodeRenderer = nodeRenderer;
        }

        public string Render(string text)
        {
            return Parser.Parse(text).Accept(NodeRenderer);
        }
    }
}
