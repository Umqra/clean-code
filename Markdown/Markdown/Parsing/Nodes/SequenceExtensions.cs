using System.Collections.Generic;

namespace Markdown.Parsing.Nodes
{
    public static class SequenceExtensions
    {
        public static int CombineElementHashCodesUsingParent<T, TParent>(this IEnumerable<T> sequence, TParent parent)
        {
            return CombineElementHashCodes(sequence, typeof(TParent).GetHashCode());
        }

        public static int CombineElementHashCodes<T>(this IEnumerable<T> sequence, int initHashValue = 0)
        {
            var hashCode = initHashValue;
            foreach (var element in sequence)
                unchecked
                {
                    hashCode = (hashCode * 397) ^ element.GetHashCode();
                }
            return hashCode;
        }
    }
}