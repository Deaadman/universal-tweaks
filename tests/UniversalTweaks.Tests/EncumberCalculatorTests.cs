using UniversalTweaks.Core;

namespace UniversalTweaks.Tests;

public class EncumberCalculatorTests
{
    [Theory]
    [InlineData(0, false, 0f)]
    [InlineData(15, false, 15f)]
    [InlineData(-5, false, 0f)]
    public void Additional_weight_is_the_setting_and_never_below_zero(int additionalEncumbermentWeight,
        bool infiniteEncumberWeight, float expected) =>
        Assert.Equal(expected,
            EncumberCalculator.ResolveAdditionalWeight(additionalEncumbermentWeight, infiniteEncumberWeight));

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    public void Infinite_carry_weight_wins_over_the_additional_weight(int additionalEncumbermentWeight) =>
        Assert.Equal(EncumberCalculator.InfiniteAdditionalWeight,
            EncumberCalculator.ResolveAdditionalWeight(additionalEncumbermentWeight, infiniteEncumberWeight: true));

    [Fact]
    public void Without_additional_weight_the_thresholds_are_the_games()
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
    public void Additional_weight_is_added_to_every_threshold()
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
