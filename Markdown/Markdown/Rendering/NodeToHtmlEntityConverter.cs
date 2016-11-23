using System;
using System.Collections.Generic;
using Markdown.Parsing.Nodes;
using Markdown.Rendering.HtmlEntities;

namespace Markdown.Rendering
{
    public class NodeToHtmlEntityConverter : INodeToHtmlEntityConverter
    {
        private Dictionary<Type, Func<INode, IHtmlTag>> internalConversionTable;
        private Dictionary<Type, Func<INode, IHtmlContent>> leafConversionTable;

        public HtmlAttribute[] CommonAttributes { get; set; }

        public NodeToHtmlEntityConverter(params HtmlAttribute[] commonAttributes)
        {
            CommonAttributes = commonAttributes;
            InitInternalConversionTable();
            InitLeafConversionTable();
        }

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

        private void InitLeafConversionTable()
        {
            leafConversionTable = new Dictionary<Type, Func<INode, IHtmlContent>>
            {
                {typeof(TextNode), node => new HtmlTextContent(((TextNode)node).Text)},
                {typeof(EscapedTextNode), node => new HtmlEscapedTextContent(((EscapedTextNode)node).Text)},
                {typeof(NewLineNode), node => new HtmlNewLineContent()}
            };
        }

        private void InitInternalConversionTable()
        {
            internalConversionTable = new Dictionary<Type, Func<INode, IHtmlTag>>
            {
                {
                    typeof(ParagraphNode), node => new BaseHtmlTag("p")
                        .AddAttributes(CommonAttributes)
                },
                {
                    typeof(EmphasisModificatorNode), node => new BaseHtmlTag("em")
                        .AddAttributes(CommonAttributes)
                },
                {
                    typeof(StrongModificatorNode), node => new BaseHtmlTag("strong")
                        .AddAttributes(CommonAttributes)
                },
                {
                    typeof(CodeModificatorNode), node => new BaseHtmlTag("pre")
                        .AddAttributes(CommonAttributes)
                },
                {typeof(GroupNode), node => new HtmlEmptyTag()},
                {
                    typeof(LinkNode),
                    node =>
                        new BaseHtmlTag("a", new HtmlAttribute("href", ((LinkNode)node).Reference))
                            .AddAttributes(CommonAttributes)
                }
            };
        }
    }
}