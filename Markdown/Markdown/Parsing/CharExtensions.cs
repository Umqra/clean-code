namespace Markdown.Parsing
{
    public static class CharExtensions
    {
        public static bool IsWhiteSpace(this char? symbol)
        {
            return symbol.HasValue && char.IsWhiteSpace(symbol.Value);
        }

        public static bool IsLetterOrDigit(this char? symbol)
        {
            return symbol.HasValue && char.IsLetterOrDigit(symbol.Value);
        }
    }
}