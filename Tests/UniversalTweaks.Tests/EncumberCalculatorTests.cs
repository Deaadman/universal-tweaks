using UniversalTweaks.Utilities;

using Xunit;

namespace UniversalTweaks.Tests;

public class EncumberCalculatorTests
{
    [Theory]
    [InlineData(0, false, 0f)]
    [InlineData(15, false, 15f)]
    [InlineData(-5, false, 0f)]
    [InlineData(15, true, 9970f)]
    [InlineData(0, true, 9970f)]
    public void ResolveAdditionalWeight_ReturnsExpectedWeight(int additionalEncumbermentWeight, bool infiniteEncumberWeight, float expected)
    {
        var result = EncumberCalculator.ResolveAdditionalWeight(additionalEncumbermentWeight, infiniteEncumberWeight);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateThresholds_WithNoAdditionalWeight_MatchesBaseValues()
    {
        var thresholds = EncumberCalculator.CalculateThresholds(0f);

        Assert.Equal(30f, thresholds.MaxCarryCapacity);
        Assert.Equal(15f, thresholds.MaxCarryCapacityWhenExhausted);
        Assert.Equal(40f, thresholds.NoSprintCarryCapacity);
        Assert.Equal(60f, thresholds.NoWalkCarryCapacity);
        Assert.Equal(31f, thresholds.EncumberLowThreshold);
        Assert.Equal(40f, thresholds.EncumberMedThreshold);
        Assert.Equal(60f, thresholds.EncumberHighThreshold);
    }

    [Fact]
    public void CalculateThresholds_AddsAdditionalWeightToEveryThreshold()
    {
        var thresholds = EncumberCalculator.CalculateThresholds(20f);

        Assert.Equal(50f, thresholds.MaxCarryCapacity);
        Assert.Equal(35f, thresholds.MaxCarryCapacityWhenExhausted);
        Assert.Equal(60f, thresholds.NoSprintCarryCapacity);
        Assert.Equal(80f, thresholds.NoWalkCarryCapacity);
        Assert.Equal(51f, thresholds.EncumberLowThreshold);
        Assert.Equal(60f, thresholds.EncumberMedThreshold);
        Assert.Equal(80f, thresholds.EncumberHighThreshold);
    }
}