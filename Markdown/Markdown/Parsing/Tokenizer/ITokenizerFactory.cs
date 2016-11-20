namespace Markdown.Parsing.Tokenizer
{
    public interface ITokenizerFactory<out T> where T : class
    {
        ITokenizer<T> CreateTokenizer(string text);
    }
}