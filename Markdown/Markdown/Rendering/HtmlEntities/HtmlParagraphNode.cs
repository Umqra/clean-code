namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlParagraphNode : IHtmlTag
    {
        public string OpeningTag => "<p>";
        public string ClosingTag => "</p>";
    }
}