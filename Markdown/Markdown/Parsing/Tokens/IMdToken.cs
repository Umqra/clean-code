namespace Markdown.Parsing.Tokens
{
    public interface IMdToken
    {
        string Text { get; }
        string UnderlyingText { get; }
        bool Has(params Md[] attribute);
        IMdToken With(params Md[] attribute);
    }
}