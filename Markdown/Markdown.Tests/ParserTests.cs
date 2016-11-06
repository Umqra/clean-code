using System.ComponentModel;
using FluentAssertions;
using Markdown.Parsing;
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
        }

        public MarkdownParser Parser { get; set; }

        [Test]
        public void TestSimpleText()
        {
            var sample = "sample text";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(Paragraph(Text(sample)));
        }

        [Test]
        public void TestItalicUnderscore()
        {
            var sample = "_sample text_";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(LowEmphasisText(Text("sample text"))));
        }

        [Test]
        public void TestBoldUnderscore()
        {
            var sample = "__sample text__";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(MediumEmphasisText(Text("sample text"))));
        }

        [Test]
        public void TestConsecutiveModificators()
        {
            var sample = "_first_ __second__ _third_";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(
                    LowEmphasisText(Text("first")),
                    Text(" "),
                    MediumEmphasisText(Text("second")),
                    Text(" "),
                    LowEmphasisText(Text("third"))
                ));
        }

        [Test]
        public void ItalicInBold_ShouldBeParsed()
        {
            var sample = "__bold _italic_ end__";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(
                    MediumEmphasisText(
                        Text("bold "),
                        LowEmphasisText(Text("italic")),
                        Text(" end"))
                ));
        }

        [Test]
        public void BoldInItalic_ShouldBeParsed()
        {
            var sample = "_italic __bold__ end_";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(
                    LowEmphasisText(
                        Text("italic "),
                        MediumEmphasisText(Text("bold")),
                        Text(" end"))
                ));
        }

        [Test]
        public void NotPairedUnderscores_ShouldNotModifyText()
        {
            var sample = "_a";
            var parsed = Parser.Parse(sample);

            parsed.Should().Be(
                Paragraph(
                    Group(
                        Text("_"),
                        Text("a"))
                ));
        }
    }
}