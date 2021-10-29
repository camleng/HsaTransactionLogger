using System;
using Business.Parsers;
using FluentAssertions;
using Xunit;

namespace Business.Tests.Parsers;

public class DateParserTests
{
    [Fact]
    public void GivenNonDate_ItReturnsAnUnsuccessfulResult()
    {
        var result = DateParser.Parse("not a date", out _);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void GivenValidDate_ItReturnsASuccessfulResult()
    {
        var result = DateParser.Parse("10/24/1994", out _);
        result.Success.Should().BeTrue();
    }
    
    [Fact]
    public void GivenValidDate_ItSetsTheOutVariable()
    {
        DateParser.Parse("10/24/1994", out var date);
        date.Should().Be(new DateTime(1994, 10, 24));
    }
}