using Markdown.Parsing.Nodes;

namespace Markdown.Parsing.Visitors
{
    public abstract class ContextTreeVisitor<T> : INodeVisitor where T : ITreeContext
    {
        // Nit: Do you really need a setter?
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
            // Nit: If you use if with braces, else should be with braces too.
            else
                Context.EnterLeafNode(node);
        }
    }
}