using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Markdown.Parsing.Tokens;

namespace Markdown.Tests
{
    public static class FluentAssertionsExtension
    {
        public static void BeEqualToFoldedSequence(this GenericCollectionAssertions<IToken> self, params IEnumerable<IToken>[] tokens)
        {
            var expectedTokens = tokens.SelectMany(x => x).ToList();
            var actualTokens = self.Subject.ToList();
            Execute.Assertion
                .ForCondition(expectedTokens.SequenceEqual(actualTokens))
                .FailWith("Expected token sequence: {0}, but found {1}", expectedTokens, actualTokens);
        }
    }
}
