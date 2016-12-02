using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests.Parsing
{
    internal class BaseMarkdownTokenizerTests
    {
        public ITokenizer<IMdToken> Tokenizer { get; set; }

        public void SetUpTokenizer(string text)
        {
            Tokenizer = new MarkdownTokenizer(text);
        }

        public IEnumerable<IMdToken> GetAllTokens()
        {
            while (!Tokenizer.AtEnd)
            {
                yield return Tokenizer.CurrentToken;
                Tokenizer = Tokenizer.Advance();
            }
        }

        protected IEnumerable<IMdToken> Token(string text, params Md[] attributes)
        {
            yield return new MdToken(text).With(attributes);
        }

        protected IEnumerable<IMdToken> Escaped(string text)
        {
            yield return new MdToken(text).With(Md.Escaped);
        }

        protected IEnumerable<IMdToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new MdToken(new string(c, 1)).With(Md.PlainText);
        }

        protected IEnumerable<IMdToken> OpenEmphasis(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Emphasis);
        }

        protected IEnumerable<IMdToken> CloseEmphasis(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Emphasis);
        }

        protected IEnumerable<IMdToken> OpenStrong(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Strong);
        }

        protected IEnumerable<IMdToken> CloseStrong(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Strong);
        }

        protected IEnumerable<IMdToken> OpenCode(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Code);
        }

        protected IEnumerable<IMdToken> CloseCode(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Code);
        }

        protected IEnumerable<IMdToken> Break(string text)
        {
            yield return new MdToken(text).With(Md.Break);
        }

        protected readonly string TwoSpaces_NewLine = "  " + Environment.NewLine;
        protected readonly string NewLine_NewLine = Environment.NewLine + Environment.NewLine;
    }

    [TestFixture]
    internal class MarkdownTokenizer_Should : BaseMarkdownTokenizerTests
    {
        [TestCase("sample", TestName = "When passed simple plain text")]
        [TestCase("1_a_b_a2_a", TestName = "When underscores surrounded by letters/digits")]
        public void ParseOnlyTextAndEscaped(string text)
        {
            SetUpTokenizer(text);

            foreach (var token in GetAllTokens())
                (token.Has(Md.PlainText) || token.Has(Md.Escaped)).Should().BeTrue();
        }

        [Test]
        public void ParseEscapeSequence()
        {
            var text = @"a\\\_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a"), Escaped("\\"), Escaped("_")
            );
        }

        [Test]
        public void ParseStrongThenEmphasis_FromTripleUnderscore()
        {
            var text = "___triple___";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                OpenStrong("__"),
                OpenEmphasis("_"),
                PlainText("triple"),
                CloseStrong("__"),
                CloseEmphasis("_")
            );
        }

        internal class ParseCodeModificator : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromBackticks_AtTheEndOfSentence()
            {
                var text = "this is `code`.";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("this is "),
                    OpenCode("`"),
                    PlainText("code"),
                    CloseCode("`"),
                    PlainText("."));
            }

            [Test]
            public void FromBackticks_OnTextBorders()
            {
                var text = "`text`";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    OpenCode("`"), PlainText("text"), CloseCode("`")
                );
            }

            [Test]
            public void FromBackticks_OnWordBorders()
            {
                var text = "a `b c` d";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("a "), OpenCode("`"), PlainText("b c"), CloseCode("`"), PlainText(" d")
                );
            }
            
            [Test]
            public void FromBackticks_SurroundedWithPunctuation()
            {
                var text = "this is `#include<iostream>` code";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("this is "),
                    OpenCode("`"),
                    PlainText("#include<iostream>"),
                    CloseCode("`"),
                    PlainText(" code")
                );
            }
        }

        internal class ParseStrongModificator : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromDoubleUnderscore_AtTheEndOfSentence()
            {
                var text = "This is the __end__.";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("This is the "),
                    OpenStrong("__"),
                    PlainText("end"),
                    CloseStrong("__"),
                    PlainText(".")
                );
            }

            [Test]
            public void FromDoubleUnderscore_OnTextBorders()
            {
                var text = "__a__";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    OpenStrong("__"), PlainText("a"), CloseStrong("__")
                );
            }

            [Test]
            public void FromDoubleUnderscore_OnWordBorders()
            {
                var text = "a __b c__ d";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("a "), OpenStrong("__"), PlainText("b c"), CloseStrong("__"), PlainText(" d")
                );
            }

            [Test]
            public void FromDoubleUnderscore_SurroundedWithPunctuation()
            {
                var text = "this is __!important!__.";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("this is "),
                    OpenStrong("__"),
                    PlainText("!important!__.")
                );
            }

            [Test]
            public void FromDoubleUnderscore_SurroundedWithSpaces()
            {
                var text = " __ ";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText(" "), OpenEmphasis("_"), CloseEmphasis("_"), PlainText(" "));
            }
        }

        internal class ParseIndentToken : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromFourSpaces_AtTheBeginningOfLine()
            {
                var text = "    text";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    Token("    ", Md.Indent), PlainText("text")
                );
            }

            [Test]
            public void FromTabulation_AtTheBeginningOfLine()
            {
                var text = "\ttext";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    Token("\t", Md.Indent), PlainText("text"));
            }

            [Test]
            public void FromFourSpaces_AfterNewLine()
            {
                var text = @"a
    b";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("a"), Token(Environment.NewLine, Md.NewLine), Token("    ", Md.Indent), PlainText("b"));
            }
        }

        internal class ParseLinkTokens : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromPairedSquareBrackets()
            {
                var text = "[link]";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    Token("[", Md.Open, Md.LinkText),
                    PlainText("link"),
                    Token("]", Md.Close, Md.LinkText)
                );
            }

            [Test]
            public void FromPairedParentheses()
            {
                var text = "(link)";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    Token("(", Md.Open, Md.LinkReference),
                    PlainText("link"),
                    Token(")", Md.Close, Md.LinkReference)
                );
            }
        }

        internal class ParseHeaderToken : BaseMarkdownTokenizerTests
        {
            [TestCase("#")][TestCase("##")]
            [TestCase("###")][TestCase("####")]
            [TestCase("#####")][TestCase("######")]
            public void FromHashes_AtTheBeginningOfLine(string hashes)
            {
                var text = $"{hashes} header";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    Token(hashes, Md.Header),
                    PlainText("header")
                );
            }
        }

        internal class ParseEmphasisModificator : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromSingleUnderscore_AtTheEndOfSentence()
            {
                var text = "This is the _end_.";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("This is the "),
                    OpenEmphasis("_"),
                    PlainText("end"),
                    CloseEmphasis("_"),
                    PlainText(".")
                );
            }

            [Test]
            public void FromSingleUnderscore_OnTextBorders()
            {
                var text = "_a_";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    OpenEmphasis("_"), PlainText("a"), CloseEmphasis("_")
                );
            }

            [Test]
            public void FromSingleUnderscore_OnWordBorders()
            {
                var text = "a _b c_ d";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("a "), OpenEmphasis("_"), PlainText("b c"), CloseEmphasis("_"), PlainText(" d")
                );
            }

            [Test]
            public void FromSingleUnderscore_SurroundedByPunctuation()
            {
                var text = "this is _!important!_.";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("this is "), OpenEmphasis("_"), PlainText("!important!_.")
                );
            }

            [Test]
            public void FromSingleUnderscore_SurroundedWithSpaces()
            {
                var text = " _ ";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText(" _ "));
            }
        }

        internal class ParseBreakToken : BaseMarkdownTokenizerTests
        {
            [Test]
            public void FromTwoNewLines()
            {
                var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("hello"), Break(NewLine_NewLine), PlainText("bye")
                );
            }

            [Test]
            public void FromTwoSpaces_AtTheEndOfLine()
            {
                var text = $"hello  {Environment.NewLine}bye";
                SetUpTokenizer(text);

                GetAllTokens().Should().BeEqualToFoldedSequence(
                    PlainText("hello"), Break(TwoSpaces_NewLine), PlainText("bye")
                );
            }
        }

        [Test]
        public void NotParse_LinkTokens_FromUnmatchedSquaredBracket()
        {
            var text = "[link";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("[", Md.Open, Md.LinkText),
                PlainText("link")
            );
        }

        [Test]
        public void NotParse_LinkTokens_FromUnmatchedParenthesis()
        {
            var text = "(link";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("(", Md.Open, Md.LinkReference),
                PlainText("link")
            );
        }

        [TestCase("#######")]
        [TestCase("########")]
        public void NotParseHeaderToken_FromManyHashes(string hashes)
        {
            var text = $"{hashes} header";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(text));
        }
    }
}