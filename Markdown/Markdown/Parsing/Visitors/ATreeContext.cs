using System.Collections.Generic;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public abstract class ATreeContext : ITreeContext
    {
        // CR: Order
        protected ATreeContext()
        {
            EnteredNodes = new Stack<INode>();
        }

        private Stack<INode> EnteredNodes { get; }

        public void Dispose()
        {
            ExitInternalNode(EnteredNodes.Pop());
        }

        // CR: No visibility modificator
        ITreeContext ITreeContext.EnterInternalNode(INode node)
        {
            EnteredNodes.Push(node);
            EnterInternalNode(node);
            return this;
        }

        // CR: No visibility modificator
        void ITreeContext.EnterLeafNode(INode node)
        {
            EnterLeafNode(node);
        }

        protected abstract void EnterInternalNode(INode node);
        protected abstract void ExitInternalNode(INode node);
        protected abstract void EnterLeafNode(INode node);
    }
}