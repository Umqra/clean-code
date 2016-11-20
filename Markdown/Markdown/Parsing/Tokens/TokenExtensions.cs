using System.Linq;

namespace Markdown.Parsing.Tokens
{
    public static class TokenExtensions
    {
        public static bool HasAny(this IMdToken token, params Md[] attributes)
        {
            return attributes.Any(attribute => token.Has(attribute));
        }
    }
}