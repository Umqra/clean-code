using System;
using System.Collections.Generic;
using Markdown.Parsing.Nodes;
using Markdown.Rendering.HtmlEntities;

namespace Markdown.Rendering
{
    public class NodeHtmlRendererFactory : INodeHtmlRendererFactory
    {
        private static readonly Dictionary<Type, Func<INode, IHtmlTag>> internalConversionTable = new
            Dictionary<Type, Func<INode, IHtmlTag>>
            {
                {typeof(ParagraphNode), node => new HtmlParagraphNode()},
                {typeof(EmphasisTextNode), node => new HtmlEmphasisTag()}
            };

        private static readonly Dictionary<Type, Func<INode, IHtmlContent>> leafConversionTable = new
            Dictionary<Type, Func<INode, IHtmlContent>>
            {
                {typeof(TextNode), node => new HtmlTextContent(((TextNode)node).Text)},
                {typeof(EscapedTextNode), node => new HtmlEscapedTextContent(((EscapedTextNode)node).Text)},
                {typeof(NewLineNode), node => new HtmlNewLineContent()}
            };

        public IHtmlTag CreateInternal(INode node)
        {
            return internalConversionTable[node.GetType()](node);
        }

        public IHtmlContent CreateLeaf(INode node)
        {
            return leafConversionTable[node.GetType()](node);
        }
    }
}