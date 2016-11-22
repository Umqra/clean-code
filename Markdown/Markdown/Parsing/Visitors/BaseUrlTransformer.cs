using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public class BaseUrlTransformer : INodeTransformer
    {
        public string BaseUrl { get; }

        public BaseUrlTransformer(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        private bool IsRelativeUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Relative, out result);
        }

        public void Transform(INode node)
        {
            if (node is LinkNode)
            {
                var linkNode = (LinkNode)node;
                if (IsRelativeUrl(linkNode.Reference))
                    linkNode.Reference = BaseUrl + linkNode.Reference;
            }
        }
    }
}