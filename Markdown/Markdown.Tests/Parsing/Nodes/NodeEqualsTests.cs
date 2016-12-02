using FluentAssertions;
using Markdown.Tests.Parsing;
using NUnit.Framework;

namespace Markdown.Tests.Parsing.Nodes
{
    [TestFixture]
    internal class NodeEqualsTests : BaseTreeTests
    {
        [Test]
        public void TestEqualsEmphasisModifierNodes()
        {
            var a = EmphasisModifier(Text("a"), Text("b"));
            var b = EmphasisModifier(Text("a"), Text("b"));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsGroupNodes()
        {
            var a = Group(Text("a"), Text("a"));
            var b = Group(Text("a"), Text("a"));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsParagraphNodes()
        {
            var a = Paragraph(EmphasisModifier(Text("a")), StrongModifier(Text("b")));
            var b = Paragraph(EmphasisModifier(Text("a")), StrongModifier(Text("b")));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsStrongModifierNodes()
        {
            var a = StrongModifier(Text("a"), Text("b"));
            var b = StrongModifier(Text("a"), Text("b"));

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
        public void TestNotEqualsEmphasisModifierNodes()
        {
            var a = EmphasisModifier(Text("a"), Text("b"));
            var b = EmphasisModifier(Text("b"), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsGroupNodes()
        {
            var a = Group(Text("a"), Text("b"));
            var b = Group(Text("b"), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsParagraphNodes()
        {
            var a = Paragraph(EmphasisModifier(Text("a")), EmphasisModifier(Text("b")));
            var b = Paragraph(EmphasisModifier(Text("b")), EmphasisModifier(Text("a")));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsStrongModifierNodes()
        {
            var a = StrongModifier(Text("a"), Text("b"));
            var b = StrongModifier(Text("b"), Text("a"));

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