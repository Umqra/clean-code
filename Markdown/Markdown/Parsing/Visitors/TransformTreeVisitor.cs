using System;
using System.Linq;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public class TransformTreeVisitor : INodeVisitor
    {
        public Action<INode> TransformAction { get; }

        public TransformTreeVisitor(INodeTransformer transformer)
        {
            TransformAction = transformer.Transform;
        }

        public TransformTreeVisitor(Action<INode> transformer)
        {
            TransformAction = transformer;
        }

        public void Visit(INode node)
        {
            if (node is IInternalNode)
            {
                var internalNode = (IInternalNode)node;
                foreach (var child in internalNode.Children)
                    Visit(child);
            }
            TransformAction(node);
        }
    }
}