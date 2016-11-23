namespace Markdown.Parsing.Tokenizer
{
    public interface ITokenizer<out T> where T : class
    {
        T CurrentToken { get; }
        bool AtEnd { get; }
        ITokenizer<T> Advance();
    }
}