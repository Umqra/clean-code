using Markdown.Parsing;
using Markdown.Parsing.Tokens;

namespace Markdown.Rendering
{
    public class MarkdownToHtmlRenderer
    {
        public MarkdownParser Parser { get; set; }
        public ITokenizerFactory<IMdToken> Tokenizer { get; set; }
        public INodeRenderer NodeRenderer { get; set; }

        public MarkdownToHtmlRenderer(MarkdownParser parser, ITokenizerFactory<IMdToken> tokenizer,
            INodeRenderer nodeRenderer)
        {
            Parser = parser;
            Tokenizer = tokenizer;
            NodeRenderer = nodeRenderer;
        }

        public string Render(string text)
        {
            return NodeRenderer.Render(Parser.Parse(new MarkdownTokenizer(text)).Parsed);
        }
    }
}