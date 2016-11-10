using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web;
using Markdown.Parsing.Nodes;

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

    public class HtmlEscapedTextContent : IHtmlContent
    {
        public HtmlEscapedTextContent(string text)
        {
            Content = HttpUtility.HtmlEncode(text);
        }

        public string Content { get; }
    }

    public class HtmlNewLineContent : IHtmlContent
    {
        public string Content => "<br>";
    }

    public class HtmlTextContent : IHtmlContent
    {
        public HtmlTextContent(string text)
        {
            Content = text;
        }

        public string Content { get; }
    }

    public class HtmlEmphasisTag : IHtmlTag
    {
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";
    }

    public class HtmlParagraphNode : IHtmlTag
    {
        public string OpeningTag => "<p>";
        public string ClosingTag => "</p>";
    }
}