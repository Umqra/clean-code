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
                var node = Tokenizer.GetNextToken();
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

        private IEnumerable<IToken> OpenModificator(string modificator)
        {
            yield return new OpenModificatorToken(modificator);
        }

        private IEnumerable<IToken> CloseModificator(string modificator)
        {
            yield return new CloseModificatorToken(modificator);
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
                OpenModificator("__")
                .Concat(Character('a'))
                .Concat(CloseModificator("__")));
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
                    .Concat(OpenModificator("__"))
                    .Concat(PlainText("b c"))
                    .Concat(CloseModificator("__"))
                    .Concat(PlainText(" d"))
                );
        }

        [Test]
        public void SingleUnderscore_DetectedOnTextBorders()
        {
            var text = "_a_";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                OpenModificator("_")
                .Concat(Character('a'))
                .Concat(CloseModificator("_")));
        }

        [Test]
        public void SingleUnderscore_DetectedOnWordBorders()
        {
            var text = "a _b c_ d";
            Tokenizer = new TextTokenizer(text);

            GetAllTokens().Should().Equal(
                PlainText("a ")
                    .Concat(OpenModificator("_"))
                    .Concat(PlainText("b c"))
                    .Concat(CloseModificator("_"))
                    .Concat(PlainText(" d")));
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
