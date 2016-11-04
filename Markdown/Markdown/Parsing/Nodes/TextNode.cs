namespace Markdown.Parsing.Nodes
{
    public class TextNode : INode
    {
        public string Text { get; set; }

        public TextNode(string text)
        {
            Text = text;
        }

        public T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}