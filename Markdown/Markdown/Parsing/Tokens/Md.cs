namespace Markdown.Parsing.Tokens
{
    public enum Md
    {
        Open,
        Close,
        Emphasis,
        Strong,
        Code,
        Escaped,
        PlainText,
        Break,
        LinkReference,
        LinkText,
        Header,
        NewLine,
    }
}