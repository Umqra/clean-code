﻿namespace Markdown.Parsing.Nodes
{
    public class TextNode : INode
    {
        public string Text { get; }

        public TextNode(string text)
        {
            Text = text;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TextNode)obj);
        }

        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }

        protected bool Equals(TextNode other)
        {
            return string.Equals(Text, other.Text);
        }
    }
}