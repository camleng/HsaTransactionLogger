using System.Collections.Generic;
using Business.Extractors;
using FluentAssertions;
using Xunit;

namespace Business.Tests.Extractors;

public class AmountExtractorTests
{
    [Fact]
    public void GivenAnEmptyList_ReturnsAnEmptyList()
    {
        var lines = new List<string>();
        var amounts = AmountExtractor.ExtractAmountsFrom(lines);
        amounts.Should().BeEmpty();
    }
    
    [Fact]
    public void GivenAListOfNonAmountStrings_ReturnsAnEmptyList()
    {
        var lines = new List<string> { "Cameron", "Abby", "Leo" };
        var amounts = AmountExtractor.ExtractAmountsFrom(lines);
        amounts.Should().BeEmpty();
    }
    
    [Fact]
    public void GivenAListOfAmountStrings_ReturnsAmountsAsDecimals()
    {
        var lines = new List<string> { "$24.94", "36.62" };
        var amounts = AmountExtractor.ExtractAmountsFrom(lines);
        amounts.Should().BeEquivalentTo(
            new List<decimal> { 24.94m, 36.62m });
    }
    
    [Fact]
    public void GivenAListOfAmountAndNonAmountStrings_ReturnsOnlyDecimals()
    {
        var lines = new List<string> { "10/24/1994", "Cameron", "$25.89", "Abby" };
        var amounts = AmountExtractor.ExtractAmountsFrom(lines);
        amounts.Should().BeEquivalentTo(
            new List<decimal> { 25.89m });
    }
}
