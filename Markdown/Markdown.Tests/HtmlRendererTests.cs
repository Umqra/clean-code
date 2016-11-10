﻿using FluentAssertions;
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
            Renderer = new HtmlRenderer();
        }

        public INodeRenderer Renderer;

        public string Render(INode node)
        {
            return Renderer.Render(node);
        }

        [Test]
        public void TestEmptyEmphasisModificator()
        {
            var node = EmphasisModificator();

            Render(node).Should().Be("<em></em>");
        }

        [Test]
        public void TestEmptyStrongModificator()
        {
            var node = StrongModificator();

            Render(node).Should().Be("<strong></strong>");
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
        public void TestManyNodesInEmphasisModificatorNode()
        {
            var node = EmphasisModificator(Text("first"), StrongModificator(Text("second")));

            Render(node).Should().Be("<em>first<strong>second</strong></em>");
        }

        [Test]
        public void TestManyNodesInStrongModificatorNode()
        {
            var node = StrongModificator(Text("first"), EmphasisModificator(Text("second")));

            Render(node).Should().Be("<strong>first<em>second</em></strong>");
        }

        [Test]
        public void TestManyNodesInParagraphNode()
        {
            var node = Paragraph(
                Text("first"),
                StrongModificator(Text("second")),
                EmphasisModificator(Text("third")));

            Render(node).Should().Be("<p>first<strong>second</strong><em>third</em></p>");
        }

        [Test]
        public void TestNewLineNode()
        {
            var node = NewLine();

            Render(node).Should().Be("<br>");
        }

        [Test]
        public void TestTextNode()
        {
            var node = Text("sample");

            Render(node).Should().Be("sample");
        }

        [Test]
        public void TestGroupNode()
        {
            var node = Group();

            Render(node).Should().Be("");
        }
    }
}