using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parsing.Tokenizer;
using Markdown.Parsing.Tokenizer.Input;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Markdown.Tests.Parsing.Tokenizer
{
    [TestFixture]
    public class StreamInput_Should
    {
        public StreamReader CreateStreamFromString(string text)
        {
            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(text)));
        }

        public IInput CreateStreamInputFromString(string text, int maxDistance)
        {
            return new StreamInput(CreateStreamFromString(text), maxDistance);
        }

        [Test]
        public void ImmediatelyEnds_WhenStreamIsEmpty()
        {
            using (var input = CreateStreamInputFromString("", 1))
                input.AtEnd.Should().BeTrue();
        }

        [Test]
        public void LookAheadOnSmallDistances()
        {
            using (var input = CreateStreamInputFromString("hello", 3))
                input.LookAhead(1).Should().Be('e');
        }

        [Test]
        public void LookBehindOnSmallDistances()
        {
            using (var input = CreateStreamInputFromString("hello", 3))
                input.LookBehind(1).Should().Be(null);
        }

        [Test]
        public void Ends_WhenCurrentSymbol_DoesNotExist()
        {
            using (var input = CreateStreamInputFromString("abc", 4))
            {
                var next = input.Advance(3);

                next.AtEnd.Should().BeTrue();
            }
        }

        [Test]
        public void NotEnds_WhenCurrentSymbol_Exists()
        {
            using (var input = CreateStreamInputFromString("abc", 4))
            {
                var next = input.Advance(2);

                next.AtEnd.Should().BeFalse();
            }
        }

        [Test]
        public void ThrowException_WhenTryLookAhead_TooFar()
        {
            Action act = () =>
            {
                using (var input = CreateStreamInputFromString("hello", 3))
                    input.LookAhead(4);
            };

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ThrowException_WhenTryLookBehind_TooFar()
        {
            Action act = () =>
            {
                using (var input = CreateStreamInputFromString("hello", 3))
                    input.LookBehind(4);
            };

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void LookAheadOnSmallDistances_AfterAdvance()
        {
            using (var input = CreateStreamInputFromString("hello", 3))
            {
                var next = input.Advance(1);
                next.LookAhead(1).Should().Be('l');
            }
        }

        [Test]
        public void LookBehindOnSmallDistances_AfterAdvance()
        {
            using (var input = CreateStreamInputFromString("hello", 3))
            {
                var next = input.Advance(1);
                next.LookBehind(1).Should().Be('h');
            }
        }

        [Test]
        public void ReturnNull_IfRequestedSymbol_DoesNotExist()
        {
            using (var input = CreateStreamInputFromString("hello", 10))
            {
                input.LookAhead(10).Should().Be(null);
            }
        }

        [Test]
        public void ReturnRightCharacter_AfterTwoAdvances()
        {
            using (var input = CreateStreamInputFromString("0123456789", 3))
            {
                var next = input.Advance(4);
                next = next.Advance(2);
                next.LookAhead(3).Should().Be('9');
                next.LookBehind(3).Should().Be('3');
            }
        }

        [Test]
        public void TruncateQueryLength_IfStreamNearEnd()
        {
            using (var input = CreateStreamInputFromString("01", 3))
            {
                input.LookAtString(3).Should().Be("01");
            }
        }
    }
}
