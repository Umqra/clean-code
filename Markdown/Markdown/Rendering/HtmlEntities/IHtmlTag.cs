namespace Markdown.Rendering.HtmlEntities
{
    public interface IHtmlTag
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
    }
}