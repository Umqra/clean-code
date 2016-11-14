namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlCodeTag : IHtmlTag
    {
        public string OpeningTag => "<pre>";
        public string ClosingTag => "</pre>";
    }
}