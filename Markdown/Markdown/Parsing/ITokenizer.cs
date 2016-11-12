using System;
using System.Collections.Generic;

namespace Markdown.Parsing
{
    public interface ITokenizer<T> where T : class
    {
        TSpec TakeTokenIfMatch<TSpec>() where TSpec : class, T;
        TSpec TakeTokenIfMatch<TSpec>(Predicate<TSpec> matchPredicate) where TSpec : class, T;
        List<T> TakeTokensUntilMatch(Predicate<T> matchPredicate);
    }
}