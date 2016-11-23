namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlAttribute
    {
        public string Name { get; }
        public string Value { get; }

        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}=\"{Value}\"";
        }
    }
}