using System.Web;

namespace Markdown.Rendering.HtmlEntities
{
    public class HtmlEscapedTextContent : IHtmlContent
    {
        public HtmlEscapedTextContent(string text)
        {
            Content = HttpUtility.HtmlEncode(text);
        }

        public string Content { get; }
    }
}