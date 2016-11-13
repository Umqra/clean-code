using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public interface ITokenizer<T> where T : class
    {
        T TakeTokenIfMatch(Predicate<T> matchPredicate);
        List<T> TakeTokensUntilMatch(Predicate<T> matchPredicate);
    }
}