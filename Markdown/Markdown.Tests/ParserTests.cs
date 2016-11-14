using System;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class ParserTests : BaseTreeTests
    {
        [SetUp]
        public void SetUp()
        {
            Parser = new MarkdownParser();
            TokenizerFactory = new MarkdownTokenizerFactory();
        }

        public MarkdownParser Parser { get; set; }
        public ITokenizerFactory<IMdToken> TokenizerFactory { get; set; }

        [Test]
        public void BoldInItalic_ShouldBeParsed()
        {
            var text = "_italic __bold__ end_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    EmphasisModificator(
                        Text("italic "),
                        StrongModificator(Text("bold")),
                        Text(" end"))
                )
            );
        }

        [Test]
        public void BoldWithUnmatchedUnderscoreInside_ShouldBeParsed()
        {
            var text = "__bold _still bold end__";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    StrongModificator(Text("bold "), Group(Text("_"), Text("still bold end")))
                )
            );
        }

        [Test]
        public void ItalicInBold_ShouldBeParsed()
        {
            var text = "__bold _italic_ end__";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    StrongModificator(
                        Text("bold "),
                        EmphasisModificator(Text("italic")),
                        Text(" end"))
                )
            );
        }

        [Test]
        public void NotPairedUnderscore_ShouldNotModifyText()
        {
            var text = "_a";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(Group(Text("_"), Text("a"))));
        }

        [Test]
        public void TestBackticks()
        {
            var text = "a `b` c";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    Text("a "), Code(Text("b")), Text(" c"))
            );
        }

        [Test]
        public void TestBoldUnderscore()
        {
            var text = "__sample text__";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(StrongModificator(Text("sample text"))));
        }

        [Test]
        public void TestConsecutiveModificators()
        {
            var text = "_first_ __second__ _third_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    EmphasisModificator(Text("first")),
                    Text(" "),
                    StrongModificator(Text("second")),
                    Text(" "),
                    EmphasisModificator(Text("third"))
                )
            );
        }

        [Test]
        public void TestEscapedCharacters()
        {
            var text = @"hi \_\_!";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    Group(
                        Text("h"), Text("i"), Text(" "), Escaped("_"), Escaped("_"), Text("!"))
                )
            );
        }

        [Test]
        public void TestItalicUnderscore()
        {
            var text = "_sample text_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(EmphasisModificator(Text("sample text"))));
        }

        [Test]
        public void TestManyOpenModificators()
        {
            var text = "a _b _c d _e";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    Text("a "),
                    Group(
                        Text("_"),
                        Text("b "),
                        Group(
                            Text("_"),
                            Text("c d "),
                            Group(Text("_"), Text("e"))
                        )
                    )
                )
            );
        }

        [Test]
        public void TestModificatorInBackticks()
        {
            var text = "a `b _c_ d` e";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    Text("a "),
                    Code(
                        Text("b "),
                        EmphasisModificator(Text("c")),
                        Text(" d")),
                    Text(" e")
                )
            );
        }

        [Test]
        public void TestSimpleText()
        {
            var text = "sample text";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(Text(text)));
        }

        [Test]
        public void TwoLineBreaks_SeparatesParagraphs()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    Paragraph(Text("bye"))
                )
            );
        }

        [Test]
        public void TwoSpacesAtTheEndOfLine_SeparatesParagraph()
        {
            var text = $"hello  {Environment.NewLine}bye";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    Paragraph(Text("bye"))
                )
            );
        }

        [Test]
        public void WhiteSpaceSymbols_AfterParagraph_NotTrimmed()
        {
            var text = "new paragraph    ";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text(text))
                )
            );
        }

        [Test]
        public void WhiteSpaceSymbols_BeforeParagraph_Trimmed()
        {
            var text = "   new paragraph";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text("new paragraph"))
                )
            );
        }
    }
}