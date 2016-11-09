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
        public ITokenizerFactory<IToken> TokenizerFactory { get; set; }

        [Test]
        public void BoldInItalic_ShouldBeParsed()
        {
            var text = "_italic __bold__ end_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    LowEmphasisText(
                        Text("italic "),
                        MediumEmphasisText(Text("bold")),
                        Text(" end"))
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
                    MediumEmphasisText(
                        Text("bold "),
                        LowEmphasisText(Text("italic")),
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
        public void TestBoldUnderscore()
        {
            var text = "__sample text__";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(MediumEmphasisText(Text("sample text"))));
        }

        [Test]
        public void TestConsecutiveModificators()
        {
            var text = "_first_ __second__ _third_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Paragraph(
                    LowEmphasisText(Text("first")),
                    Text(" "),
                    MediumEmphasisText(Text("second")),
                    Text(" "),
                    LowEmphasisText(Text("third"))
                )
            );
        }

        [Test]
        public void TestItalicUnderscore()
        {
            var text = "_sample text_";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(LowEmphasisText(Text("sample text"))));
        }

        [Test]
        public void TestSimpleText()
        {
            var text = "sample text";
            var parsed = Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(Paragraph(Text(text)));
        }

        [Test]
        public void TwoLineBreaks_IsNewLineNode()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    NewLine(),
                    Paragraph(Text("bye"))
                )
            );
        }

        [Test]
        public void TwoSpacesAtTheEndOfLine_IsNewLineNode()
        {
            var text = $"hello  {Environment.NewLine}bye";
            var parsed = Parser.Parse(TokenizerFactory.CreateTokenizer(text));

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    NewLine(),
                    Paragraph(Text("bye"))
                )
            );
        }
    }
}