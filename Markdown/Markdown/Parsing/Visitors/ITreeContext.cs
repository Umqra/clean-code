using System;
using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public interface ITreeContext : IDisposable
    {
        ITreeContext EnterInternalNode(INode node);
        void EnterLeafNode(INode node);
    }
}