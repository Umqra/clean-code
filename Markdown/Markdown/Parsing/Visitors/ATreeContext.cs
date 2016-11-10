﻿using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public abstract class ATreeContext : ITreeContext
    {
        protected ATreeContext()
        {
            EnteredNodes = new Stack<INode>();
        }

        private Stack<INode> EnteredNodes { get; }

        public void Dispose()
        {
            ExitInternalNode(EnteredNodes.Pop());
        }

        ITreeContext ITreeContext.EnterInternalNode(INode node)
        {
            EnteredNodes.Push(node);
            EnterInternalNode(node);
            return this;
        }

        void ITreeContext.EnterLeafNode(INode node)
        {
            EnterLeafNode(node);
        }

        protected abstract void EnterInternalNode(INode node);
        protected abstract void ExitInternalNode(INode node);
        protected abstract void EnterLeafNode(INode node);
    }
}