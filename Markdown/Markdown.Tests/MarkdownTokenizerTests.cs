using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class MarkdownTokenizerTests
    {
        public BaseTokenizer<IMdToken> Tokenizer { get; set; }

        public void SetUpTokenizer(string text)
        {
            Tokenizer = new MarkdownTokenizer(text);
        }

        public IEnumerable<IMdToken> GetAllTokens()
        {
            while (true)
            {
                var node = Tokenizer.TakeToken<IMdToken>();
                if (node == null)
                    yield break;
                yield return node;
            }
        }

        private IEnumerable<IMdToken> Escaped(string text)
        {
            yield return new MdEscapedTextToken(text);
        }

        private IEnumerable<IMdToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new MdTextToken(new string(c, 1));
        }

        private IEnumerable<IMdToken> Emphasis(string modificator)
        {
            yield return new MdEmphasisModificatorToken(modificator);
        }

        private IEnumerable<IMdToken> Strong(string modificator)
        {
            yield return new MdStrongModificatorToken(modificator);
        }

        private readonly string twoSpacesNewLine = "  " + Environment.NewLine;
        private readonly string twoLineBreaksNewLine = Environment.NewLine + Environment.NewLine;

        private IEnumerable<IMdToken> NewLine(string text)
        {
            yield return new MdNewLineToken(text);
        }

        [TestCase("sample", TestName = "When passed simple plain text")]
        [TestCase(" _ __\t_\n", TestName = "When underscores surrounded by white spaces")]
        [TestCase("1_a__b____a2_a", TestName = "When underscores surrounded by letters/digits")]
        public void TestOnlyCharacterTokensInText(string text)
        {
            SetUpTokenizer(text);

            foreach (var token in GetAllTokens())
                token.Should().BeOfType<MdTextToken>();
        }

        [Test]
        public void TestDoubleUnderscore_AtTheEndOfSentence()
        {
            var text = "This is the __end__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("This is the "), Strong("__"), PlainText("end"), Strong("__"), PlainText(".")
            );
        }

        [Test]
        public void TestDoubleUnderscore_OnTextBorders()
        {
            var text = "__a__";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Strong("__"), PlainText("a"), Strong("__")
            );
        }

        [Test]
        public void TestDoubleUnderscore_OnWordBorders()
        {
            var text = "a __b c__ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), Strong("__"), PlainText("b c"), Strong("__"), PlainText(" d")
            );
        }

        [Test]
        public void TestDoubleUnderscore_SurroundedByPunctuation()
        {
            var text = "this is __!important!__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("this is "), Strong("__"), PlainText("!important!"), Strong("__"), PlainText(".")
            );
        }

        [Test]
        public void TestSingleUnderscore_AtTheEndOfSentence()
        {
            var text = "This is the _end_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("This is the "), Emphasis("_"), PlainText("end"), Emphasis("_"), PlainText(".")
            );
        }

        [Test]
        public void TestSingleUnderscore_OnTextBorders()
        {
            var text = "_a_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Emphasis("_"), PlainText("a"), Emphasis("_")
            );
        }

        [Test]
        public void TestSingleUnderscore_OnWordBorders()
        {
            var text = "a _b c_ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), Emphasis("_"), PlainText("b c"), Emphasis("_"), PlainText(" d")
            );
        }

        [Test]
        public void TestSingleUnderscore_SurroundedByPunctuation()
        {
            var text = "this is _!important!_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("this is "), Emphasis("_"), PlainText("!important!"), Emphasis("_"), PlainText(".")
            );
        }

        [Test]
        public void TestEscapedCharacter()
        {
            var text = @"a\\\_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a"), Escaped("\\"), Escaped("_")
            );
        }

        [Test]
        public void TestTripleUnderscore()
        {
            var text = "___triple___";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(text)
            );
        }

        [Test]
        public void TestTwoLineBreaks()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), NewLine(twoLineBreaksNewLine), PlainText("bye")
            );
        }

        [Test]
        public void TestTwoSpaces_AtTheEndOfLine()
        {
            var text = $"hello  {Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), NewLine(twoSpacesNewLine), PlainText("bye")
            );
        }
    }
}