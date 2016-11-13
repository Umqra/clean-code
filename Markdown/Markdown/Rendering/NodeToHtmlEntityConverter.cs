using System;
using System.Collections.Generic;
using Markdown.Parsing.Nodes;
using Markdown.Rendering.HtmlEntities;

namespace Markdown.Rendering
{
    public class NodeToHtmlEntityConverter : INodeToHtmlEntityConverter
    {
        private static readonly Dictionary<Type, Func<INode, IHtmlTag>> internalConversionTable = new
            Dictionary<Type, Func<INode, IHtmlTag>>
            {
                {typeof(ParagraphNode), node => new HtmlParagraphTag()},
                {typeof(EmphasisModificatorNode), node => new HtmlEmphasisTag()},
                {typeof(StrongModificatorNode), node => new HtmlStrongTag()},
                {typeof(CodeModificatorNode), node => new HtmlCodeTag()},
                {typeof(GroupNode), node => new HtmlEmptyTag()},
            };

        private static readonly Dictionary<Type, Func<INode, IHtmlContent>> leafConversionTable = new
            Dictionary<Type, Func<INode, IHtmlContent>>
            {
                {typeof(TextNode), node => new HtmlTextContent(((TextNode)node).Text)},
                {typeof(EscapedTextNode), node => new HtmlEscapedTextContent(((EscapedTextNode)node).Text)},
                {typeof(NewLineNode), node => new HtmlNewLineContent()}
            };

        public IHtmlTag ConvertInternal(INode node)
        {
            try
            {
                return internalConversionTable[node.GetType()](node);
            }
            catch (KeyNotFoundException exception)
            {
                throw new ArgumentException(
                    $"No conversion rule for internal node {node}.",
                    exception);
            }
        }

        public IHtmlContent ConvertLeaf(INode node)
        {
            try
            {
                return leafConversionTable[node.GetType()](node);
            }
            catch (KeyNotFoundException exception)
            {
                throw new ArgumentException(
                    $"No conversion rule for leaf node {node}. {exception.Message}",
                    exception);
            }
        }
    }
}