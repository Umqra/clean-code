namespace Markdown.Rendering
{
    public interface IHtmlTag
    {
        string OpeningTag { get; }
        string ClosingTag { get; }
    }
}