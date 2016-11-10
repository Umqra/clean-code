namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlEmptyTag : IHtmlTag
    {
        public string OpeningTag => "";
        public string ClosingTag => "";
    }
}