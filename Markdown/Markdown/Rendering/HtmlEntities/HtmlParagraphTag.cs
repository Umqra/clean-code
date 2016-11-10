namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlParagraphTag : IHtmlTag
    {
        public string OpeningTag => "<p>";
        public string ClosingTag => "</p>";
    }
}