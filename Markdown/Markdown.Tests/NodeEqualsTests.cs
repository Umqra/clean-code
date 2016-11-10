using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class NodeEqualsTests : BaseTreeTests
    {
        [Test]
        public void TestEqualsEmphasisModificatorNodes()
        {
            var a = EmphasisModificator(Text("a"), Text("b"));
            var b = EmphasisModificator(Text("a"), Text("b"));

            a.Equals(b).Should().BeTrue();
        }

        [Test]
        public void TestEqualsStrongModificatorNodes()
        {
            var a = StrongModificator(Text("a"), Text("b"));
            var b = StrongModificator(Text("a"), Text("b"));

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
            var a = Paragraph(EmphasisModificator(Text("a")), StrongModificator(Text("b")));
            var b = Paragraph(EmphasisModificator(Text("a")), StrongModificator(Text("b")));

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
        public void TestNotEqualsGroupNodes()
        {
            var a = Group(Text("a"), Text("b"));
            var b = Group(Text("b"), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsParagraphNodes()
        {
            var a = Paragraph(EmphasisModificator(Text("a")), EmphasisModificator(Text("b")));
            var b = Paragraph(EmphasisModificator(Text("b")), EmphasisModificator(Text("a")));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsTextNodes()
        {
            var a = Text("first");
            var b = Text("second");

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsEmphasisModificatorNodes()
        {
            var a = EmphasisModificator(Text("a"), Text("b"));
            var b = EmphasisModificator(Text("b"), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsStrongModificatorNodes()
        {
            var a = StrongModificator(Text("a"), Text("b"));
            var b = StrongModificator(Text("b"), Text("a"));

            a.Equals(b).Should().BeFalse();
        }
    }
}