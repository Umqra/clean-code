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
    internal class ParserTests : BaseTreeTests
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
        public void Backticks_ShouldBeParsed()
        {
            var text = "a `b` c";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("a "), Code(Text("b")), Text(" c"))
            );
        }

        [Test]
        public void Backticks_ShouldIgnoreModificators()
        {
            var text = "a `__b__` c";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Text("a "), Code(Text("_"), Text("_"), Text("b"), Text("_"), Text("_")), Text(" c"))
            );
        }

        [Test]
        public void BackticksContents_ShouldBeParsed_CharByChar()
        {
            var text = "`a b c`";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Code(Text("a"), Text(" "), Text("b"), Text(" "), Text("c"))
                ));
        }

        [Test]
        public void BoldInItalic_ShouldBeParsed()
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
        public void PairedModificators_ShouldNotParsed_IfBordersWithPunctuation()
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
        public void BoldItalic_ShouldParsed_WhenDifferentTypeUsed()
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
        public void BoldUnderscore_ShouldBeParsed()
        {
            var text = "__sample text__";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(StrongModificator(Text("sample text"))));
        }

        [Test]
        public void BoldWithUnmatchedUnderscoreInside_ShouldBeParsed()
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
        public void ConsecutiveModificators_ShouldBeParsed()
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
        public void EscapedCharacters_ShouldBeParsed()
        {
            var text = @"hi \_\_!";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Group(Text("hi "), Escaped("_"), Escaped("_"), Text("!")))
            );
        }

        [Test]
        public void ItalicInBold_ShouldBeParsed()
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
        public void ItalicUnderscore_ShouldBeParsed()
        {
            var text = "_sample text_";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(EmphasisModificator(Text("sample text"))));
        }

        [Test]
        public void LinkElement_ShouldBeParsed()
        {
            var text = "[link](/index.html)";

            var parsed = ParseParagraph(text);

            parsed.Should().Be(
                Paragraph(
                    Link("/index.html", Text("link"))
                ));
        }

        [Test]
        public void ManyOpenModificators_ShouldAllBeBroken()
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
        public void NotPairedUnderscore_ShouldNotModifyText()
        {
            var text = "_a";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(Text("_"), Text("a")));
        }

        [Test]
        public void OnlyLinkReferenceElement_ShouldNotModifyText()
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
        public void OnlyLinkTextElement_ShouldNotModifyText()
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
        public void SimpleText_ShouldBeParsed()
        {
            var text = "sample text";
            var parsed = ParseParagraph(text);

            parsed.Should().Be(Paragraph(Text(text)));
        }

        [Test]
        public void TwoLineBreaks_SeparatesParagraphs()
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
        public void TwoSpacesAtTheEndOfLine_SeparatesParagraph()
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
        public void WhiteSpaceSymbols_AfterParagraph_NotTrimmed()
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
        public void WhiteSpaceSymbols_BeforeParagraph_Trimmed()
        {
            var text = "   new paragraph";
            var parsed = Parse(text);

            parsed.Should().Be(
                Group(
                    Paragraph(Text("new paragraph"))
                )
            );
        }
    }
}