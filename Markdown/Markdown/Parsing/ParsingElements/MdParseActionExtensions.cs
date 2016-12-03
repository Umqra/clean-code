using System.Collections.Generic;
using Markdown.Parsing.Tokenizer;

namespace Markdown.Parsing.ParsingElements
{
    public static class MdParseActionExtensions
    {
        public static MdTokenizerAction Then(this MdTokenizerAction first, MdTokenizerAction second)
        {
            return new MdTokenizerAction(tokenizer => second.Do(first.Do(tokenizer)));
        }

        public static MdParserAction<T2> Then<T1, T2>(this MdParserAction<T1> first, MdParserAction<T2> second)
        {
            return new MdParserAction<T2>(tokenizer =>
            {
                var firstTokenizer = first.Do(tokenizer);
                if (first.Succeed)
                {
                    var secondTokenizer = second.Do(firstTokenizer);
                    if (second.Succeed)
                        return secondTokenizer.SuccessWith(second.Parsed);
                    return firstTokenizer.Fail<T2>();
                }
                return firstTokenizer.Fail<T2>();
            });
        }

        public static MdParserAction<T> Then<T>(this MdParserAction<T> first, MdTokenizerAction second)
        {
            return new MdParserAction<T>(tokenizer =>
            {
                var firstTokenizer = first.Do(tokenizer);
                if (first.Succeed)
                {
                    var remainder = second.Do(firstTokenizer);
                    return remainder.SuccessWith(first.Parsed);
                }
                return firstTokenizer.Fail<T>();
            });
        }

        public static MdParserAction<T> Then<T>(this MdTokenizerAction first, MdParserAction<T> second)
        {
            return new MdParserAction<T>(tokenizer =>
            {
                var secondTokenizer = second.Do(first.Do(tokenizer));
                if (second.Succeed)
                    return secondTokenizer.SuccessWith(second.Parsed);
                return secondTokenizer.Fail<T>();
            });
        }

        public static MdParserAction<T> Or<T>(this MdParserAction<T> first, MdParserAction<T> second)
        {
            return new MdParserAction<T>(tokenizer =>
            {
                var firstTokenizer = first.Do(tokenizer);
                if (first.Succeed)
                    return firstTokenizer.SuccessWith(first.Parsed);
                var secondTokenizer = second.Do(tokenizer);
                if (second.Succeed)
                    return secondTokenizer.SuccessWith(second.Parsed);
                return secondTokenizer.Fail<T>();
            });
        }

        public static MdParserAction<List<T>> UntilSucceed<T>(this MdParserAction<T> first)
        {
            return new MdParserAction<List<T>>(tokenizer =>
            {
                var parsed = new List<T>();
                while (true)
                {
                    tokenizer = first.Do(tokenizer);
                    if (!first.Succeed)
                        return tokenizer.SuccessWith(parsed);
                    parsed.Add(first.Parsed);
                }
            });
        }
    }
}