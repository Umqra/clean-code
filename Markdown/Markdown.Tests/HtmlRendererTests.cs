using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;
using Markdown.Rendering;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class HtmlRendererTests
    {
        public HtmlNodeRenderer Renderer;

        public string Render(INode node)
        {
            return node.Accept(Renderer);
        }

        [SetUp]
        public void SetUp()
        {
            Renderer = new HtmlNodeRenderer();
        }

        [Test]
        public void TestEmptyTextNode()
        {
            var node = new TextNode("");

            Render(node).Should().Be("");
        }

        [Test]
        public void TestTextNode()
        {
            var node = new TextNode("sample");

            Render(node).Should().Be("sample");
        }

        [Test]
        public void TestEmptyBoldTextNode()
        {
            var node = new BoldTextNode(new INode[] {});

            Render(node).Should().Be("<strong></strong>");
        }
        
        [Test]
        public void TestManyNodesInBoldTextNode()
        {
            var node = new BoldTextNode(new INode[]
            {
                new TextNode("first"),
                new ItalicTextNode(new [] {new TextNode("second") })  
            });

            Render(node).Should().Be("<strong>first<em>second</em></strong>");
        }

        [Test]
        public void TestEmptyItalicNode()
        {
            var node = new ItalicTextNode(new INode[] {});

            Render(node).Should().Be("<em></em>");
        }

        [Test]
        public void TestManyNodesInItalicNode()
        {
            var node = new ItalicTextNode(new INode[]
            {
                new TextNode("first"),
                new BoldTextNode(new[] {new TextNode("second")})  
            });

            Render(node).Should().Be("<em>first<strong>second</strong></em>");
        }
        

        [Test]
        public void TestEmptyParagraphNode()
        {
            var node = new ParagraphNode(new INode[] {});

            Render(node).Should().Be("<p></p>");
        }

        [Test]
        public void TestManyNodesInParagraphNode()
        {
            var node =
                new ParagraphNode(new INode[]
                {
                    new TextNode("first"),
                    new BoldTextNode(new[] {new TextNode("second")}),
                    new ItalicTextNode(new[] {new TextNode("third")})
                });

            Render(node).Should().Be("<p>first<strong>second</strong><em>third</em></p>");
        }
    }
}
