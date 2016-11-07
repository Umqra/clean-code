using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;
using Markdown.Rendering;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class HtmlRendererTests : BaseTreeTests
    {
        [SetUp]
        public void SetUp()
        {
            Renderer = new HtmlNodeRenderer();
        }

        public HtmlNodeRenderer Renderer;

        public string Render(INode node)
        {
            return node.Accept(Renderer);
        }

        [Test]
        public void TestEmptyHighEmphasisTextNode()
        {
            var node = HighEmphasisText();

            Render(node).Should().Be("<b></b>");
        }

        [Test]
        public void TestEmptyMediumEmphasisTextNode()
        {
            var node = MediumEmphasisText();

            Render(node).Should().Be("<strong></strong>");
        }

        [Test]
        public void TestEmptyLowEmphasisNode()
        {
            var node = LowEmphasisText();

            Render(node).Should().Be("<em></em>");
        }


        [Test]
        public void TestEmptyParagraphNode()
        {
            var node = Paragraph();

            Render(node).Should().Be("<p></p>");
        }

        [Test]
        public void TestEmptyTextNode()
        {
            var node = Text("");

            Render(node).Should().Be("");
        }

        [Test]
        public void TestManyNodesInHighEmphasisTextNode()
        {
            var node = HighEmphasisText(Text("first"), LowEmphasisText(Text("second")));

            Render(node).Should().Be("<b>first<em>second</em></b>");
        }

        [Test]
        public void TestManyNodesInMediumEmphasisTextNode()
        {
            var node = MediumEmphasisText(Text("first"), LowEmphasisText(Text("second")));

            Render(node).Should().Be("<strong>first<em>second</em></strong>");
        }

        [Test]
        public void TestManyNodesInLowEmphasisNode()
        {
            var node = LowEmphasisText(Text("first"), MediumEmphasisText(Text("second")));

            Render(node).Should().Be("<em>first<strong>second</strong></em>");
        }

        [Test]
        public void TestManyNodesInParagraphNode()
        {
            var node = Paragraph(
                Text("first"),
                MediumEmphasisText(Text("second")),
                LowEmphasisText(Text("third")));

            Render(node).Should().Be("<p>first<strong>second</strong><em>third</em></p>");
        }

        [Test]
        public void TestTextNode()
        {
            var node = Text("sample");

            Render(node).Should().Be("sample");
        }
    }
}