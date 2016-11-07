using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class NodeEqualsTests : BaseTreeTests
    {
        [Test]
        public void TestEqualsEmphasisTextNodes()
        {
            var a = MediumEmphasisText(Text("a"), LowEmphasisText(Text("b")), HighEmphasisText(Text("c")));
            var b = MediumEmphasisText(Text("a"), LowEmphasisText(Text("b")), HighEmphasisText(Text("c")));

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
            var a = Paragraph(LowEmphasisText(Text("a")), MediumEmphasisText(Text("b")));
            var b = Paragraph(LowEmphasisText(Text("a")), MediumEmphasisText(Text("b")));

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
        public void TestNotEqualsEmphasisTextNodes_WhenOrderDiffers()
        {
            var a = MediumEmphasisText(Text("a"), LowEmphasisText(Text("b")));
            var b = MediumEmphasisText(LowEmphasisText(Text("b")), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsEmphasisTextNodes_WhenStrengthDiffers()
        {
            var a = MediumEmphasisText(Text("a"), LowEmphasisText(Text("b")));
            var b = HighEmphasisText(Text("a"), LowEmphasisText(Text("b")));

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
        public void TestNotEqualsItalicTextNodes()
        {
            var a = LowEmphasisText(Text("a"), MediumEmphasisText(Text("b")));
            var b = LowEmphasisText(MediumEmphasisText(Text("b")), Text("a"));

            a.Equals(b).Should().BeFalse();
        }

        [Test]
        public void TestNotEqualsParagraphNodes()
        {
            var a = Paragraph(LowEmphasisText(Text("a")), LowEmphasisText(Text("b")));
            var b = Paragraph(LowEmphasisText(Text("b")), LowEmphasisText(Text("a")));

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