﻿using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public abstract class ContextTreeVisitor<T> : INodeVisitor where T : ITreeContext
    {
        protected T Context { get; }

        protected ContextTreeVisitor(T context)
        {
            Context = context;
        }

        public void Visit(INode node)
        {
            var internalNode = node as IInternalNode;
            if (internalNode != null)
            {
                using (Context.EnterInternalNode(node))
                {
                    foreach (var child in internalNode.Children)
                        Visit(child);
                }
            }
            else
            {
                Context.EnterLeafNode(node);
            }
        }
    }
}