using Markdown.Parsing.Tokens;

namespace Markdown.Parsing
{
    public class MarkdownTokenizerFactory : ITokenizerFactory<IMdToken>
    {
        public ATokenizer<IMdToken> CreateTokenizer(string text)
        {
            return new MarkdownTokenizer(text);
        }
    }
}