namespace Markdown.Rendering.HtmlEntities
{
    internal class HtmlCodeTag : IHtmlTag
    {
        public string OpeningTag => "<pre>";
        public string ClosingTag => "</pre>";
    }
}