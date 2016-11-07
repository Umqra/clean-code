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
    class MarkdownTokenizerTests
    {
        public ATokenizer<IToken> Tokenizer { get; set; }

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

        private IEnumerable<IToken> NewLine()
        {
            yield return new NewLineToken();
        }

        [TestCase("sample", TestName = "When passed simple plain text")]
        [TestCase(" _ __\t_\n", TestName = "When underscores surrounded by white spaces")]
        [TestCase("1_a__b____a2_a", TestName = "When underscores surrounded by letters/digits")]
        public void TestOnlyCharacterTokensInText(string text)
        {
            Tokenizer = new MarkdownTokenizer().ForText(text);

            foreach (var token in GetAllTokens())
                token.Should().BeOfType<CharacterToken>();
        }

        [Test]
        public void TestEscapedCharacter()
        {
            var text = @"a\\\_";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                Character('a')
                .Concat(Escaped('\\'))
                .Concat(Escaped('_')));
        }

        [Test]
        public void DoubleUnderscoreOnTextBorders_IsModificator()
        {
            var text = "__a__";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                Modificator("__")
                .Concat(Character('a'))
                .Concat(Modificator("__")));
        }

        [Test]
        public void DoubleUnderscoreOnWordBorders_IsModificator()
        {
            var text = "a __b c__ d";
            Tokenizer = new MarkdownTokenizer().ForText(text);

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
        public void SingleUnderscoreOnTextBorders_IsModificator()
        {
            var text = "_a_";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                Modificator("_")
                .Concat(Character('a'))
                .Concat(Modificator("_")));
        }

        [Test]
        public void SingleUnderscoreOnWordBorders_IsModificator()
        {
            var text = "a _b c_ d";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("a ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("b c"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(" d")));
        }

        [Test]
        public void DoubleUnderscoreSurroundedByPunctuation_IsModificator()
        {
            var text = "this is __!important!__.";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("this is ")
                    .Concat(Modificator("__"))
                    .Concat(PlainText("!important!"))
                    .Concat(Modificator("__"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void DoubleUnderscoreAtTheEndOfSentence_IsModificator()
        {
            var text = "This is the __end__.";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("This is the ")
                    .Concat(Modificator("__"))
                    .Concat(PlainText("end"))
                    .Concat(Modificator("__"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void SingleUnderscoreSurroundedByPunctuation_IsModificator()
        {
            var text = "this is _!important!_.";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("this is ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("!important!"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void SingleUnderscoreAtTheEndOfSentence_IsModificator()
        {
            var text = "This is the _end_.";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("This is the ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("end"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(".")));
        }

        [Test]
        public void TripleUnderscore_IsItalicInBoldModificators()
        {
            var text = "___triple___";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                Modificator("___")
                    .Concat(PlainText("triple"))
                    .Concat(Modificator("___")));
        }

        [Test]
        public void TwoSpacesAtTheEndOfLine_IsNewLineToken()
        {
            var text = $"hello  {Environment.NewLine}bye";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("hello")
                    .Concat(NewLine())
                    .Concat(PlainText("bye")));
        }

        [Test]
        public void TwoLineBreaks_IsNewLineToken()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            Tokenizer = new MarkdownTokenizer().ForText(text);

            GetAllTokens().Should().Equal(
                PlainText("hello")
                    .Concat(NewLine())
                    .Concat(PlainText("bye")));
        }
    }
}
