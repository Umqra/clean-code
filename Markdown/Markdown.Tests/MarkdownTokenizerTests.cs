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
        public ATokenizer<IMdToken> Tokenizer { get; set; }

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

        private IEnumerable<IMdToken> Character(char c)
        {
            yield return new MdCharacterToken(c);
        }

        private IEnumerable<IMdToken> Escaped(char c)
        {
            yield return new MdEscapedCharacterToken(c);
        }

        private IEnumerable<IMdToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new MdCharacterToken(c);
        }

        private IEnumerable<IMdToken> Modificator(string modificator)
        {
            yield return new MdEmphasisModificatorToken(modificator);
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
                token.Should().BeOfType<MdCharacterToken>();
        }

        [Test]
        public void DoubleUnderscore_AtTheEndOfSentence_IsModificator()
        {
            var text = "This is the __end__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("This is the "), Modificator("__"), PlainText("end"), Modificator("__"), PlainText(".")
            );
        }

        [Test]
        public void DoubleUnderscore_OnTextBorders_IsModificator()
        {
            var text = "__a__";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Modificator("__"), Character('a'), Modificator("__")
            );
        }

        [Test]
        public void DoubleUnderscore_OnWordBorders_IsModificator()
        {
            var text = "a __b c__ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), Modificator("__"), PlainText("b c"), Modificator("__"), PlainText(" d")
            );
        }

        [Test]
        public void DoubleUnderscore_SurroundedByPunctuation_IsModificator()
        {
            var text = "this is __!important!__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("this is "), Modificator("__"), PlainText("!important!"), Modificator("__"), PlainText(".")
            );
        }

        [Test]
        public void SingleUnderscore_AtTheEndOfSentence_IsModificator()
        {
            var text = "This is the _end_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("This is the "), Modificator("_"), PlainText("end"), Modificator("_"), PlainText(".")
            );
        }

        [Test]
        public void SingleUnderscore_OnTextBorders_IsModificator()
        {
            var text = "_a_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Modificator("_"), Character('a'), Modificator("_")
            );
        }

        [Test]
        public void SingleUnderscore_OnWordBorders_IsModificator()
        {
            var text = "a _b c_ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("a "), Modificator("_"), PlainText("b c"), Modificator("_"), PlainText(" d")
            );
        }

        [Test]
        public void SingleUnderscore_SurroundedByPunctuation_IsModificator()
        {
            var text = "this is _!important!_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("this is "), Modificator("_"), PlainText("!important!"), Modificator("_"), PlainText(".")
            );
        }

        [Test]
        public void TestEscapedCharacter()
        {
            var text = @"a\\\_";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Character('a'), Escaped('\\'), Escaped('_')
            );
        }

        [Test]
        public void TripleUnderscore_IsItalicInBoldModificators()
        {
            var text = "___triple___";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                Modificator("___"), PlainText("triple"), Modificator("___")
            );
        }

        [Test]
        public void TwoLineBreaks_IsNewLineToken()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), NewLine(twoLineBreaksNewLine), PlainText("bye")
            );
        }

        [Test]
        public void TwoSpaces_AtTheEndOfLine_IsNewLineToken()
        {
            var text = $"hello  {Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().BeEqualToFoldedSequence(
                PlainText("hello"), NewLine(twoSpacesNewLine), PlainText("bye")
            );
        }
    }
}