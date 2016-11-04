using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Rendering
{
    class HtmlNodeRenderer : INodeRenderer
    {
        public string Visit(ParagraphNode node)
        {
            throw new NotImplementedException();
        }

        public string Visit(BoldTextNode node)
        {
            throw new NotImplementedException();
        }

        public string Visit(ItalicTextNode node)
        {
            throw new NotImplementedException();
        }

        public string Visit(TextNode node)
        {
            throw new NotImplementedException();
        }
    }
}
