using Markdown.Parsing.Nodes;
using Markdown.Parsing.Visitors;

namespace Markdown.Parsing
{
    public abstract class ContextTreeVisitor<T> : INodeVisitor where T : ITreeContext
    {
        protected T Context { get; set; }

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
                Context.EnterLeafNode(node);
        }
    }
}