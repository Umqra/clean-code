namespace Markdown.Parsing.Tokens
{
    public interface IMdToken
    {
        string Text { get; }
        string UnderlyingText { get; }
        bool Has(params Md[] attributes);
        IMdToken With(params Md[] attributes);
    }
}