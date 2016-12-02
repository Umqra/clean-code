using FluentAssertions;
using Markdown.Parsing.Nodes;
using Markdown.Rendering;
using Markdown.Rendering.HtmlEntities;
using Markdown.Tests.Parsing;
using NUnit.Framework;

namespace Markdown.Tests.Rendering
{
    [TestFixture]
    internal class NodeHtmlRenderer_Should : BaseTreeTests
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
        public void InsertClassAttrInTag()
        {
            Renderer = new NodeHtmlRenderer(new HtmlRenderContext(
                new NodeToHtmlEntityConverter(new HtmlAttribute("class", "test"))
            ));
            var node = StrongModificator(Text("test"));

            Render(node).Should().Be(@"<strong class=""test"">test</strong>");
        }

        [Test]
        public void SaveContext_BetweenCalls()
        {
            var node = Text("sample");

            Render(node);
            Render(node).Should().Be("samplesample");
        }

        [Test]
        public void Render_CodeNode()
        {
            var node = Code(Text("sample"));

            Render(node).Should().Be("<pre>sample</pre>");
        }

        [Test]
        public void Render_EmphasisModificator()
        {
            var node = EmphasisModificator(Text("sample"));

            Render(node).Should().Be("<em>sample</em>");
        }


        [Test]
        public void Render_ParagraphNode()
        {
            var node = Paragraph(Text("sample"));

            Render(node).Should().Be("<p>sample</p>");
        }

        [Test]
        public void Render_StrongModificator()
        {
            var node = StrongModificator(Text("sample"));

            Render(node).Should().Be("<strong>sample</strong>");
        }

        [Test]
        public void Render_TextNode()
        {
            var node = Text("sample");

            Render(node).Should().Be("sample");
        }

        [Test]
        public void Render_GroupNode()
        {
            var node = Group(Text("a"), Text("b"));

            Render(node).Should().Be("ab");
        }
        
        [Test]
        public void Render_NewLineNode()
        {
            var node = NewLine();

            Render(node).Should().Be("<br>");
        }
    }
}