using Business.Parsers;
using FluentAssertions;
using Xunit;

namespace Business.Tests.Parsers;

public class AmountParserTests
{
    [Fact]
    public void GivenNonAmount_ItReturnsAnUnsuccessfulResult()
    {
        var result = AmountParser.Parse("NonAmount", out _);
        result.Success.Should().BeFalse();
    }
    
    [Fact]
    public void GivenAAmountWithAPrependedDollarSign_ItReturnsASuccessfulResult()
    {
        var result = AmountParser.Parse("$12.18", out _);
        result.Success.Should().BeTrue();
    }
    
    [Fact]
    public void GivenAAmountWithOutAPrependedDollarSign_ItReturnsASuccessfulResult()
    {
        var result = AmountParser.Parse("12.18", out _);
        result.Success.Should().BeTrue();
    }
    
    [Fact]
    public void GivenAValidAmount_ItSetsTheOutVariable()
    {
        AmountParser.Parse("12.18", out var amount);
        amount.Should().Be(12.18m);
    }
}