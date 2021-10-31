using System;
using System.Collections.Generic;
using Business.Extractors;
using FluentAssertions;
using Xunit;

namespace Business.Tests.Extractors
{
    public class DateExtractorTests
    {
        [Fact]
        public void GivenAnEmptyList_ReturnsAnEmptyList()
        {
            var lines = new List<string>();
            var dates = DateExtractor.ExtractDatesFrom(lines);
            dates.Should().BeEmpty();
        }

        [Fact]
        public void GivenAListOfNonDates_ReturnsAnEmptyList()
        {
            var lines = new List<string> { "Cameron", "Abby", "Leo" };
            var dates = DateExtractor.ExtractDatesFrom(lines);
            dates.Should().BeEmpty();
        }

        [Fact]
        public void GivenAListOfDateStrings_ReturnsDateOnlyTypes()
        {
            var lines = new List<string> { "10/24/1994", "01/20/2001" };
            var dates = DateExtractor.ExtractDatesFrom(lines);
            dates.Should().BeEquivalentTo(
                new List<DateTime> { new(1994, 10, 24), new(2001, 1, 20) });
        }

        [Fact]
        public void GivenAListOfDateAndNonDateStrings_ReturnsOnlyDateOnlyTypes()
        {
            var lines = new List<string> { "10/24/1994", "Cameron", "01/20/2001", "Abby" };
            var dates = DateExtractor.ExtractDatesFrom(lines);
            dates.Should().BeEquivalentTo(
                new List<DateTime> { new(1994, 10, 24), new(2001, 1, 20) });
        }
    }
}