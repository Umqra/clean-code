using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class MarkdownTokenizerTests
    {
        public ATokenizer<IToken> Tokenizer { get; set; }

        public void SetUpTokenizer(string text)
        {
            Tokenizer = new MarkdownTokenizer(text);
        }

        public IEnumerable<IToken> GetAllTokens()
        {
            while (true)
            {
                var node = Tokenizer.TakeToken<IToken>();
                if (node == null)
                    yield break;
                yield return node;
            }
        }

        private IEnumerable<IToken> Character(char c)
        {
            yield return new CharacterToken(c);
        }

        private IEnumerable<IToken> Escaped(char c)
        {
            yield return new EscapedCharacterToken(c);
        }

        private IEnumerable<IToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new CharacterToken(c);
        }

        private IEnumerable<IToken> Modificator(string modificator)
        {
            yield return new EmphasisModificatorToken(modificator);
        }

        private readonly string twoSpacesNewLine = "  " + Environment.NewLine;
        private readonly string twoLineBreaksNewLine = Environment.NewLine + Environment.NewLine;

        private IEnumerable<IToken> NewLine(string text)
        {
            yield return new NewLineToken(text);
        }

        [TestCase("sample", TestName = "When passed simple plain text")]
        [TestCase(" _ __\t_\n", TestName = "When underscores surrounded by white spaces")]
        [TestCase("1_a__b____a2_a", TestName = "When underscores surrounded by letters/digits")]
        public void TestOnlyCharacterTokensInText(string text)
        {
            SetUpTokenizer(text);

            foreach (var token in GetAllTokens())
                token.Should().BeOfType<CharacterToken>();
        }

        [Test]
        public void DoubleUnderscore_AtTheEndOfSentence_IsModificator()
        {
            var text = "This is the __end__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("This is the ")
                    .Concat(Modificator("__"))
                    .Concat(PlainText("end"))
                    .Concat(Modificator("__"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void DoubleUnderscore_OnTextBorders_IsModificator()
        {
            var text = "__a__";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                Modificator("__")
                    .Concat(Character('a'))
                    .Concat(Modificator("__")));
        }

        [Test]
        public void DoubleUnderscore_OnWordBorders_IsModificator()
        {
            var text = "a __b c__ d";
            SetUpTokenizer(text);

            GetAllTokens()
                .Should()
                .Equal(
                    PlainText("a ")
                        .Concat(Modificator("__"))
                        .Concat(PlainText("b c"))
                        .Concat(Modificator("__"))
                        .Concat(PlainText(" d"))
                );
        }

        [Test]
        public void DoubleUnderscore_SurroundedByPunctuation_IsModificator()
        {
            var text = "this is __!important!__.";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("this is ")
                    .Concat(Modificator("__"))
                    .Concat(PlainText("!important!"))
                    .Concat(Modificator("__"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void SingleUnderscore_AtTheEndOfSentence_IsModificator()
        {
            var text = "This is the _end_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("This is the ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("end"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void SingleUnderscore_OnTextBorders_IsModificator()
        {
            var text = "_a_";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                Modificator("_")
                    .Concat(Character('a'))
                    .Concat(Modificator("_")));
        }

        [Test]
        public void SingleUnderscore_OnWordBorders_IsModificator()
        {
            var text = "a _b c_ d";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("a ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("b c"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(" d")));
        }

        [Test]
        public void SingleUnderscore_SurroundedByPunctuation_IsModificator()
        {
            var text = "this is _!important!_.";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("this is ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("!important!"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void TestEscapedCharacter()
        {
            var text = @"a\\\_";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                Character('a')
                    .Concat(Escaped('\\'))
                    .Concat(Escaped('_')));
        }

        [Test]
        public void TripleUnderscore_IsItalicInBoldModificators()
        {
            var text = "___triple___";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                Modificator("___")
                    .Concat(PlainText("triple"))
                    .Concat(Modificator("___")));
        }

        [Test]
        public void TwoLineBreaks_IsNewLineToken()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("hello")
                    .Concat(NewLine(twoLineBreaksNewLine))
                    .Concat(PlainText("bye")));
        }

        [Test]
        public void TwoSpaces_AtTheEndOfLine_IsNewLineToken()
        {
            var text = $"hello  {Environment.NewLine}bye";
            SetUpTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("hello")
                    .Concat(NewLine(twoSpacesNewLine))
                    .Concat(PlainText("bye")));
        }
    }
}