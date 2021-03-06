﻿using System.Linq;
using FluentAssertions;
using Markdown.Parsing.Nodes;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class SequenceCollectionTests
    {
        private class FirstSampleParent
        {
        }

        private class SecondSampleParent
        {
        }

        [Test]
        public void HashCodesCombiner_DependOnElementsOrder()
        {
            var first = new[] {"1", "2"};
            var second = first.Reverse();

            first
                .CombineElementHashCodes()
                .Should()
                .NotBe(second.CombineElementHashCodes());
        }

        [Test]
        public void HashCodesCombiner_DependOnInitialValue()
        {
            var sample = new[] {"sample"};

            sample
                .CombineElementHashCodes(1)
                .Should()
                .NotBe(sample.CombineElementHashCodes(2));
        }

        [Test]
        public void HashCodesCombiner_DependOnParentType()
        {
            var sample = new[] {"sample"};
            var firstParent = new FirstSampleParent();
            var secondParent = new SecondSampleParent();

            sample
                .CombineElementHashCodesUsingParent(firstParent)
                .Should()
                .NotBe(sample.CombineElementHashCodesUsingParent(secondParent));
        }
    }
}