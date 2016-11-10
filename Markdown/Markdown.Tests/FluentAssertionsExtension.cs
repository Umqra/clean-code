using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Markdown.Parsing.Tokens;

namespace Markdown.Tests
{
    public static class FluentAssertionsExtension
    {
        public static void BeEqualToFoldedSequence(this GenericCollectionAssertions<IMdToken> self,
            params IEnumerable<IMdToken>[] tokens)
        {
            var expectedTokens = tokens.SelectMany(x => x).ToList();
            var actualTokens = self.Subject.ToList();
            Execute.Assertion
                .ForCondition(expectedTokens.SequenceEqual(actualTokens))
                .FailWith("Expected token sequence: {0}, but found {1}. {2}",
                    expectedTokens, actualTokens,
                    DetectInequalityReason(expectedTokens, actualTokens));
        }

        public static string DetectInequalityReason(List<IMdToken> expected, List<IMdToken> actual)
        {
            if (expected.Count != actual.Count)
                return "Because sequences has different lengths";
            for (int i = 0; i < expected.Count; i++)
            {
                if (!Equals(expected[i], actual[i]))
                    return $"Because sequences differs at position {i}: {expected[i]} != {actual[i]}";
            }
            return "";
        }
    }
}