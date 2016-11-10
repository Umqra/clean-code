namespace Markdown.Parsing
{
    public interface ITokenizerFactory<T> where T : class
    {
        ATokenizer<T> CreateTokenizer(string text);
    }
}