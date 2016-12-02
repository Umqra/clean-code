using System.Collections.Generic;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class MarkdownToHtmlRenderer
    {
        public readonly List<INodeVisitor> Modifiers;
        public MarkdownParser Parser { get; }
        public ITokenizerFactory<IMdToken> Tokenizer { get; }
        public INodeRenderer NodeRenderer { get; }

        public MarkdownToHtmlRenderer(MarkdownParser parser, ITokenizerFactory<IMdToken> tokenizer,
            INodeRenderer nodeRenderer)
        {
            Parser = parser;
            Tokenizer = tokenizer;
            NodeRenderer = nodeRenderer;
            Modifiers = new List<INodeVisitor>();
        }

        public MarkdownToHtmlRenderer WithModifiers(params INodeVisitor[] modifiers)
        {
            Modifiers.AddRange(modifiers);
            return this;
        }

        public string Render(string text)
        {
            var tree = Parser.Parse(Tokenizer.CreateTokenizer(text)).Parsed;
            foreach (var modifier in Modifiers)
                modifier.Visit(tree);
            return NodeRenderer.Render(tree);
        }
    }
}