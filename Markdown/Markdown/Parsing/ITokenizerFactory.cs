namespace Markdown.Parsing
{
    public interface ITokenizerFactory<T> where T : class
    {
        ITokenizer<T> CreateTokenizer(string text);
    }
}