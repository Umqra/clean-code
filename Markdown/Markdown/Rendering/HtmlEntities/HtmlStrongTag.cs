namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlStrongTag : IHtmlTag
    {
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";
    }
}