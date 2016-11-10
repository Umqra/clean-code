namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlTextContent : IHtmlContent
    {
        public HtmlTextContent(string text)
        {
            Content = text;
        }

        public string Content { get; }
    }
}