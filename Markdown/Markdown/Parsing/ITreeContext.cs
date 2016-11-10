using System;
using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing
{
    public interface ITreeContext : IDisposable
    {
        ITreeContext EnterInternalNode(INode node);
        ITreeContext EnterLeafNode(INode node);
    }
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

        protected abstract void EnterInternalNode(INode node);
        protected abstract void ExitInternalNode(INode node);
        protected abstract void EnterLeafNode(INode node);

        ITreeContext ITreeContext.EnterInternalNode(INode node)
        {
            EnteredNodes.Push(node);
            EnterInternalNode(node);
            return this;
        }

        ITreeContext ITreeContext.EnterLeafNode(INode node)
        {
            EnterLeafNode(node);
            return this;
        }
    }
}