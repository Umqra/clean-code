using FluentAssertions;
using Markdown.Parsing.Nodes;
using Markdown.Rendering;
using Markdown.Rendering.HtmlEntities;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class HtmlRendererTests : BaseTreeTests
    {
        [SetUp]
        public void SetUp()
        {
            Renderer = new NodeHtmlRenderer(new HtmlRenderContext(new NodeToHtmlEntityConverter()));
        }

        public INodeRenderer Renderer;

        public string Render(INode node)
        {
            return Renderer.Render(node);
        }

        [Test]
        public void RendererInsertClassInTags()
        {
            Renderer = new NodeHtmlRenderer(new HtmlRenderContext(
                new NodeToHtmlEntityConverter(new HtmlAttribute("class", "test"))
            ));
            var node = StrongModificator(Text("test"));

            Render(node).Should().Be(@"<strong class=""test"">test</strong>");
        }

        [Test]
        public void RendererSaveContextBetweenCalls()
        {
            var node = Text("sample");

            Render(node);
            Render(node).Should().Be("samplesample");
        }

        [Test]
        public void TestCodeNode()
        {
            var node = Code(Text("sample"));

            Render(node).Should().Be("<pre>sample</pre>");
        }

        [Test]
        public void TestEmptyEmphasisModificator()
        {
            var node = EmphasisModificator();

            Render(node).Should().Be("<em></em>");
        }


        [Test]
        public void TestEmptyParagraphNode()
        {
            var node = Paragraph();

            Render(node).Should().Be("<p></p>");
        }

        [Test]
        public void TestEmptyStrongModificator()
        {
            var node = StrongModificator();

            Render(node).Should().Be("<strong></strong>");
        }

        [Test]
        public void TestEmptyTextNode()
        {
            var node = Text("");

            Render(node).Should().Be("");
        }

        [Test]
        public void TestGroupNode()
        {
            var node = Group();

            Render(node).Should().Be("");
        }

        [Test]
        public void TestManyNodesInEmphasisModificatorNode()
        {
            var node = EmphasisModificator(Text("first"), StrongModificator(Text("second")));

            Render(node).Should().Be("<em>first<strong>second</strong></em>");
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
        public void TestManyNodesInStrongModificatorNode()
        {
            var node = StrongModificator(Text("first"), EmphasisModificator(Text("second")));

            Render(node).Should().Be("<strong>first<em>second</em></strong>");
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
    }
}