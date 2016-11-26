using System.Collections.Generic;
using Markdown.Parsing;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;
using Markdown.Parsing.Visitors;

namespace Markdown.Rendering
{
    public class MarkdownToHtmlRenderer
    {
        // CR (krait): Более правильное слово - modifiers.
        public List<INodeVisitor> Modificators;
        public MarkdownParser Parser { get; }
        public ITokenizerFactory<IMdToken> Tokenizer { get; }
        public INodeRenderer NodeRenderer { get; }

        public MarkdownToHtmlRenderer(MarkdownParser parser, ITokenizerFactory<IMdToken> tokenizer,
            INodeRenderer nodeRenderer)
        {
            Parser = parser;
            Tokenizer = tokenizer;
            NodeRenderer = nodeRenderer;
            Modificators = new List<INodeVisitor>();
        }

        public MarkdownToHtmlRenderer WithModificators(params INodeVisitor[] modificators)
        {
            Modificators.AddRange(modificators);
            return this;
        }

        public string Render(string text)
        {
            var tree = Parser.Parse(Tokenizer.CreateTokenizer(text)).Parsed;
            foreach (var modificator in Modificators)
                modificator.Visit(tree);
            return NodeRenderer.Render(tree);
        }
    }
}