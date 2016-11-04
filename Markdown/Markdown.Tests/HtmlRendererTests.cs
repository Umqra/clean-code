using FluentAssertions;
using Markdown.Parsing;
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
        public void TestEmptyBoldTextNode()
        {
            var node = BoldText();

            Render(node).Should().Be("<strong></strong>");
        }

        [Test]
        public void TestEmptyItalicNode()
        {
            var node = ItalicText();

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
        public void TestManyNodesInBoldTextNode()
        {
            var node = BoldText(Text("first"), ItalicText(Text("second")));

            Render(node).Should().Be("<strong>first<em>second</em></strong>");
        }

        [Test]
        public void TestManyNodesInItalicNode()
        {
            var node = ItalicText(Text("first"), BoldText(Text("second")));

            Render(node).Should().Be("<em>first<strong>second</strong></em>");
        }

        [Test]
        public void TestManyNodesInParagraphNode()
        {
            var node = Paragraph(
                Text("first"),
                BoldText(Text("second")),
                ItalicText(Text("third")));

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