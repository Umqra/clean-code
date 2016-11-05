using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Tokens;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class TextTokenizerTests
    {
        public TextTokenizer Tokenizer { get; set; }

        #region Auxiliary functions for comfortable unit-testing
        public IEnumerable<IToken> GetAllTokens()
        {
            while (true)
            {
                var node = Tokenizer.TakeNextToken();
                if (node == null)
                    yield break;
                yield return node;
            }
        }

        private IEnumerable<IToken> Character(char c)
        {
            yield return new CharacterToken(c);
        }

        private IEnumerable<IToken> PlainText(string text)
        {
            foreach (var c in text)
                yield return new CharacterToken(c);
        }

        private IEnumerable<IToken> Escaped(char c)
        {
            yield return new EscapedCharacterToken(c);
        }

        private IEnumerable<IToken> Modificator(string modificator)
        {
            yield return new FormatModificatorToken(modificator);
        }
        #endregion

        [Test]
        public void TestOnlyCharacterTokens()
        {
            var text = "text";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(PlainText(text));
        }

        [Test]
        public void TestEscapedCharacter()
        {
            var text = @"a\\\_";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                Character('a')
                .Concat(Escaped('\\'))
                .Concat(Escaped('_')));
        }

        [Test]
        public void DoubleUnderscore_DetectedOnTextBorders()
        {
            var text = "__a__";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                Modificator("__")
                .Concat(Character('a'))
                .Concat(Modificator("__")));
        }

        [Test]
        public void DoubleUnderscore_DetectedOnWordBorders()
        {
            var text = "a __b c__ d";
            Tokenizer = new TextTokenizer(text);

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
        public void SingleUnderscore_DetectedOnTextBorders()
        {
            var text = "_a_";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                Modificator("_")
                .Concat(Character('a'))
                .Concat(Modificator("_")));
        }

        [Test]
        public void SingleUnderscore_DetectedOnWordBorders()
        {
            var text = "a _b c_ d";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("a ")
                    .Concat(Modificator("_"))
                    .Concat(PlainText("b c"))
                    .Concat(Modificator("_"))
                    .Concat(PlainText(" d")));
        }

        [Test]
        public void SingleUnderscore_NotDetectedInSpaces()
        {
            var text = " _ ";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText(text));
        }

        [Test]
        public void DoubleUnderscore_NotDetectedInSpaces()
        {
            var text = " __ ";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText(text));
        }

        [Test]
        public void Underscores_DetectedAsCharactersInsideWord()
        {
            var text = "a_b__1_c";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(PlainText(text));
        }
    }
}
