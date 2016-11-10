namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlEmphasisTag : IHtmlTag
    {
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";
    }
}