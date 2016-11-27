using System;
using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Nodes;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class MarkdownParser_Should : BaseTreeTests
    {
        [SetUp]
        public void SetUp()
        {
            Parser = new MarkdownParser();
            TokenizerFactory = new MarkdownTokenizerFactory();
        }

        private INode Parse(string text)
        {
            return Parser.Parse(TokenizerFactory.CreateTokenizer(text)).Parsed;
        }

        private INode ParseParagraph(string text)
        {
            return Parser.ParseParagraph(TokenizerFactory.CreateTokenizer(text)).Parsed;
        }

        public MarkdownParser Parser { get; set; }
        public ITokenizerFactory<IMdToken> TokenizerFactory { get; set; }

        [Test]
        public void ParseCodeNode_FromBackticks()
        {
            var text = "a `b` c";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("a "), Code(Escaped("b")), Text(" c"))
            );
        }

        [Test]
        public void ParseCodeNode_FromBackticks_IgnoringModificators()
        {
            var text = "a `__b__` c";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("a "), Code(Escaped("_"), Escaped("_"), Escaped("b"), Escaped("_"), Escaped("_")), Text(" c"))
            );
        }

        [Test]
        public void ParseCodeNode_FromBackticks_CharByChar()
        {
            var text = "`a b c`";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Code(Escaped("a"), Escaped(" "), Escaped("b"), Escaped(" "), Escaped("c"))
                ));
        }

        [Test]
        public void ParseEmphasisThenStrongModificators()
        {
            var text = "_italic __bold__ end_";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    EmphasisModificator(
                        Text("italic "),
                        StrongModificator(Text("bold")),
                        Text(" end"))
                )
            );
        }

        [Test]
        public void ParseNestedTextModificators_IfDifferentTypesAreUsed()
        {
            var text = "__*hello*__";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(StrongModificator(EmphasisModificator(
                    Text("hello")
                )))
            );
        }

        [Test]
        public void ParseStrongModificatorNode_FromDoubleUnderscores()
        {
            var text = "__sample text__";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(StrongModificator(Text("sample text"))));
        }

        [Test]
        public void ParseStrongModificatorNode_WithUnmatchedModificatorInside()
        {
            var text = "__bold _still bold end__";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    StrongModificator(Text("bold "), Text("_"), Text("still bold end"))
                )
            );
        }

        [Test]
        public void ParseConsecutiveModificators()
        {
            var text = "_first_ __second__ _third_";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    EmphasisModificator(Text("first")),
                    Text(" "),
                    StrongModificator(Text("second")),
                    Text(" "),
                    EmphasisModificator(Text("third"))
                )
            );
        }

        [Test]
        public void ParseEscapeNode_FromEscapeSequence()
        {
            var text = @"hi \_\_!";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Group(Text("hi "), Escaped("_"), Escaped("_"), Text("!")))
            );
        }

        [Test]
        public void ParseHeaderNode_TrimmingTrailingHashes()
        {
            var text = "## header ####";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(Header(2, Text("header ####")))
            );
        }

        [Test]
        public void ParseStrongThenEmphasisModificators()
        {
            var text = "__bold _italic_ end__";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    StrongModificator(
                        Text("bold "),
                        EmphasisModificator(Text("italic")),
                        Text(" end"))
                )
            );
        }

        [Test]
        public void ParseEmphasisModificatorNode_FromUnderscores()
        {
            var text = "_sample text_";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(EmphasisModificator(Text("sample text"))));
        }

        [Test]
        public void ParseLinkNode()
        {
            var text = "[link](/index.html)";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Link("/index.html", Text("link"))
                ));
        }

        [Test]
        public void ParseHeaderNode_FromHashes()
        {
            var text = "## header";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(Header(2, Text("header")))
            );
        }

        [Test]
        public void ParseHeaderThenParagraph_DelimitedWithTwoNewLines()
        {
            var text = @"# header

text";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(Header(1, Text("header")), Paragraph(Text("text")))
            );
        }

        [Test]
        public void ParsePlainText_FromManyNotPairedUnderscores()
        {
            var text = "a _b _c d _e";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("a "),
                    Text("_"),
                    Text("b "),
                    Text("_"),
                    Text("c d "),
                    Text("_"),
                    Text("e"))
            );
        }

        [Test]
        public void ParseHeaderThenParagraph_DelimietedWithNewLines()
        {
            var text = @"# header
text";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(Header(1, Text("header")), Paragraph(Text("text")))
            );
        }

        [Test]
        public void ParsePlainText_FromNotPairedSingleUnderscore()
        {
            var text = "_a";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(Text("_"), Text("a")));
        }

        [Test]
        public void ParsePlainText_FromLinkRefElement_WithoutLinkTextElement()
        {
            var text = "(/index.html)";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("("), Text("/index.html"), Text(")")
                )
            );
        }

        [Test]
        public void ParsePlainText_FromLinkTextElement_WithoutLinkRefElement()
        {
            var text = "[link]";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("["), Text("link"), Text("]")
                )
            );
        }

        [Test]
        public void NotParseNestedModificators_IfInnerTextEndsWithPunctuation()
        {
            var text = "__*hello!*__";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(StrongModificator(
                    Text("*"), Text("hello!*")
                ))
            );
        }

        [Test]
        public void ParsePlainText_FromPlainText()
        {
            var text = "sample text";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(Text(text)));
        }

        [Test]
        public void ParseHeaderNode_FromHashes_AtTheBeginningOfLine()
        {
            var text = "# header";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(Header(1, Text("header")))
            );
        }

        [Test]
        public void ParseCodeNode_FromFourSpaceIndentation()
        {
            var text = @"    a
    b";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(Code(Escaped("a"), Escaped(Environment.NewLine), Escaped("b"))
                )
            );
        }

        [Test]
        public void ParseTwoParagraphs_SeparatedWithTwoNewLines()
        {
            var text = $"hello{Environment.NewLine}{Environment.NewLine}bye";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    Paragraph(Text("bye"))
                )
            );
        }

        [Test]
        public void ParseTwoParagraphs_IfFirstEndsWithTwoSpaces()
        {
            var text = $"hello  {Environment.NewLine}bye";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(
                    Paragraph(Text("hello")),
                    Paragraph(Text("bye"))
                )
            );
        }

        [Test]
        public void ParseParagraph_WihoutTrimmingTrailingSpaces()
        {
            var text = "new paragraph    ";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(
                    Paragraph(Text(text))
                )
            );
        }

        [Test]
        public void ParseParagraph_WithouTrimmingLeadingSpaces()
        {
            var text = "   new paragraph";

            var parsed = Parse(text);

            parsed.Should().Be(
                Group(
                    Paragraph(Text(text))
                )
            );
        }
    }
}