﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class MarkdownTokenizerTests
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

        private IEnumerable<IMdToken> Token(string text, params Md[] attributes)
        {
            yield return new MdToken(text).With(attributes);
        }

        private IEnumerable<IMdToken> Escaped(string text)
        {
            yield return new MdToken(text).With(Md.Escaped);
        }

        private IEnumerable<IMdToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new MdToken(new string(c, 1)).With(Md.PlainText);
        }

        private IEnumerable<IMdToken> OpenEmphasis(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Emphasis);
        }

        private IEnumerable<IMdToken> CloseEmphasis(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Emphasis);
        }

        private IEnumerable<IMdToken> OpenStrong(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Strong);
        }

        private IEnumerable<IMdToken> CloseStrong(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Strong);
        }

        private IEnumerable<IMdToken> OpenCode(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Open, Md.Code);
        }

        private IEnumerable<IMdToken> CloseCode(string modificator)
        {
            yield return new MdToken(modificator).With(Md.Close, Md.Code);
        }

        private readonly string twoSpacesNewLine = "  " + Environment.NewLine;
        private readonly string twoLineBreaksNewLine = Environment.NewLine + Environment.NewLine;

        private IEnumerable<IMdToken> Break(string text)
        {
            yield return new MdToken(text).With(Md.Break);
        }

        [TestCase("sample", TestName = "When passed simple plain text")]
        [TestCase("1_a_b_a2_a", TestName = "When underscores surrounded by letters/digits")]
        public void TestOnlyCharacterTokensInText(string text)
        {
            SetUpTokenizer(text);

            foreach (var token in GetAllTokens())
                (token.Has(Md.PlainText) || token.Has(Md.Escaped)).Should().BeTrue();
        }

        [Test]
        public void TestBacktick_AtTheEndOfSentence()
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
        public void TestBacktick_OnTextBorders()
        {
            var text = "`text`";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                OpenCode("`"), PlainText("text"), CloseCode("`")
            );
        }

        [Test]
        public void TestBacktick_OnWordBorders()
        {
            var text = "a `b c` d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), OpenCode("`"), PlainText("b c"), CloseCode("`"), PlainText(" d")
            );
        }

        [Test]
        public void TestBacktick_SurroundedByPunctuation()
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

        [Test]
        public void TestDoubleUnderscore_AtTheEndOfSentence()
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
        public void TestDoubleUnderscore_OnTextBorders()
        {
            var text = "__a__";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                OpenStrong("__"), PlainText("a"), CloseStrong("__")
            );
        }

        [Test]
        public void TestDoubleUnderscore_OnWordBorders()
        {
            var text = "a __b c__ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), OpenStrong("__"), PlainText("b c"), CloseStrong("__"), PlainText(" d")
            );
        }

        [Test]
        public void TestDoubleUnderscore_SurroundedByPunctuation()
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
        public void TestDoubleUnderscoreSurroundingWithSpaces()
        {
            var text = " __ ";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(" "), OpenEmphasis("_"), CloseEmphasis("_"), PlainText(" "));
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
        public void TestIndentToken_WithFourSpaces()
        {
            var text = "    text";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("    ", Md.Indent), PlainText("text")
            );
        }

        [Test]
        public void TestIndentToken_WithTabulation()
        {
            var text = "\ttext";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("\t", Md.Indent), PlainText("text"));
        }

        [Test]
        public void TestIndentToken_AfterNewLine()
        {
            var text = @"a
    b";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a"), Token(Environment.NewLine, Md.NewLine), Token("    ", Md.Indent), PlainText("b"));
        }

        [Test]
        public void TestMatchedLinkTextTokens()
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
        public void TestMatchedLinkUrlTokens()
        {
            var text = "(link)";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("(", Md.Open, Md.LinkReference),
                PlainText("link"),
                Token(")", Md.Close, Md.LinkReference)
            );
        }

        [Test]
        public void TestMultipleHeaderToken()
        {
            var text = "## header";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("##", Md.Header),
                PlainText("header")
            );
        }

        [Test]
        public void TestSingleHeaderToken()
        {
            var text = "# header";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("#", Md.Header),
                PlainText("header")
            );
        }

        [Test]
        public void TestSingleUnderscore_AtTheEndOfSentence()
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
        public void TestSingleUnderscore_OnTextBorders()
        {
            var text = "_a_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                OpenEmphasis("_"), PlainText("a"), CloseEmphasis("_")
            );
        }

        [Test]
        public void TestSingleUnderscore_OnWordBorders()
        {
            var text = "a _b c_ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), OpenEmphasis("_"), PlainText("b c"), CloseEmphasis("_"), PlainText(" d")
            );
        }

        [Test]
        public void TestSingleUnderscore_SurroundedByPunctuation()
        {
            var text = "this is _!important!_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("this is "), OpenEmphasis("_"), PlainText("!important!_.")
            );
        }

        [Test]
        public void TestSingleUnderscoreSurroundingWithSpaces()
        {
            var text = " _ ";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(" _ "));
        }

        [Test]
        public void TestTripleUnderscore()
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

        [Test]
        public void TestTripleUnderscoreSurroundingWithSpaces()
        {
            var text = " ___ ";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(" "), OpenStrong("__"), CloseEmphasis("_"), PlainText(" "));
        }

        [Test]
        public void TestTwoLineBreaks()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), Break(twoLineBreaksNewLine), PlainText("bye")
            );
        }

        [Test]
        public void TestTwoSpaces_AtTheEndOfLine()
        {
            var text = $"hello  {Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), Break(twoSpacesNewLine), PlainText("bye")
            );
        }

        [Test]
        public void TestUnmatchedLinkTextTokens()
        {
            var text = "[link";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("[", Md.Open, Md.LinkText),
                PlainText("link")
            );
        }

        [Test]
        public void TestUnmatchedLinkUrlTokens()
        {
            var text = "(link";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Token("(", Md.Open, Md.LinkReference),
                PlainText("link")
            );
        }

        [Test]
        public void TooMuchHeaderTokens_Ignored()
        {
            var text = "####### header";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText(text));
        }
    }
}