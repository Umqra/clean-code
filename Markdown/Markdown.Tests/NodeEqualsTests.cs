using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class NodeEqualsTests : BaseTreeTests
    {
        [Test]
        public void TestEqualsBoldTextNodes()
        {
            var a = BoldText(Text("a"), ItalicText(Text("b")));
            var b = BoldText(Text("a"), ItalicText(Text("b")));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsItalicTextNodes()
        {
            var a = ItalicText(Text("a"), BoldText(Text("b")));
            var b = ItalicText(Text("a"), BoldText(Text("b")));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsParagraphNodes()
        {
            var a = Paragraph(ItalicText(Text("a")), BoldText(Text("b")));
            var b = Paragraph(ItalicText(Text("a")), BoldText(Text("b")));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsTextNodes()
        {
            var a = Text("sample");
            var b = Text("sample");

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestNotEqualsBoldTextNodes()
        {
            var a = BoldText(Text("a"), ItalicText(Text("b")));
            var b = BoldText(ItalicText(Text("b")), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsItalicTextNodes()
        {
            var a = ItalicText(Text("a"), BoldText(Text("b")));
            var b = ItalicText(BoldText(Text("b")), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsParagraphNodes()
        {
            var a = Paragraph(ItalicText(Text("a")), ItalicText(Text("b")));
            var b = Paragraph(ItalicText(Text("b")), ItalicText(Text("a")));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsTextNodes()
        {
            var a = Text("first");
            var b = Text("second");

            a.Equals(b).Should().BeFalse();
        }
    }
}