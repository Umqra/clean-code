using Markdown.Parsing;
using Markdown.Parsing.Tokens;

namespace Markdown.Rendering
{
    public class MarkdownRenderer
    {
        public MarkdownRenderer(MarkdownParser parser, ITokenizerFactory<IToken> tokenizer, INodeRenderer nodeRenderer)
        {
            Parser = parser;
            Tokenizer = tokenizer;
            NodeRenderer = nodeRenderer;
        }

        public MarkdownParser Parser { get; set; }
        public ITokenizerFactory<IToken> Tokenizer { get; set; }
        public INodeRenderer NodeRenderer { get; set; }

        public string Render(string text)
        {
            return Parser.Parse(new MarkdownTokenizer(text)).Accept(NodeRenderer);
        }
    }
}