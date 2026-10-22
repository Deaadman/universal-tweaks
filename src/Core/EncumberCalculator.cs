namespace UniversalTweaks.Core;

/// <summary>
/// The weights the player can carry before being slowed down, with the extra weight from the settings on top.
/// </summary>
internal static class EncumberCalculator
{
    /// <summary>The extra weight, in kilograms, that infinite carry weight adds: enough that it's never reached.</summary>
    internal const float InfiniteAdditionalWeight = 9970f;

    /// <summary>Gets how much extra weight, in kilograms, the player can carry.</summary>
    /// <param name="additionalEncumbermentWeight">The extra weight from the settings. Below 0 counts as 0.</param>
    /// <param name="infiniteEncumberWeight">Whether infinite carry weight is on, which wins over the extra weight.</param>
    /// <returns>The extra weight, or <see cref="InfiniteAdditionalWeight"/> with infinite carry weight.</returns>
    internal static float ResolveAdditionalWeight(int additionalEncumbermentWeight, bool infiniteEncumberWeight)
    {
        if (infiniteEncumberWeight)
            return InfiniteAdditionalWeight;

        return additionalEncumbermentWeight > 0 ? additionalEncumbermentWeight : 0f;
    }

    /// <summary>Gets the game's carry thresholds with <paramref name="additionalWeight"/> added to each.</summary>
    /// <param name="additionalWeight">The extra weight, in kilograms, from <see cref="ResolveAdditionalWeight"/>.</param>
    /// <returns>Every threshold, in kilograms.</returns>
    internal static EncumberThresholds CalculateThresholds(float additionalWeight) =>
        new(
            MaxCarryCapacity: 30f + additionalWeight,
            MaxCarryCapacityWhenExhausted: 15f + additionalWeight,
            NoSprintCarryCapacity: 40f + additionalWeight,
            NoWalkCarryCapacity: 60f + additionalWeight,
            EncumberLowThreshold: 31f + additionalWeight,
            EncumberMedThreshold: 40f + additionalWeight,
            EncumberHighThreshold: 60f + additionalWeight);
}

/// <summary>The weights, in kilograms, at which carrying more slows the player down, matching the game's <c>Encumber</c>.</summary>
/// <param name="MaxCarryCapacity">The most the player can carry before being encumbered.</param>
/// <param name="MaxCarryCapacityWhenExhausted">The most the player can carry when exhausted.</param>
/// <param name="NoSprintCarryCapacity">The weight at which the player can no longer sprint.</param>
/// <param name="NoWalkCarryCapacity">The weight at which the player can no longer walk.</param>
/// <param name="EncumberLowThreshold">Where the HUD starts showing the player as slightly encumbered.</param>
/// <param name="EncumberMedThreshold">Where the HUD shows the player as encumbered.</param>
/// <param name="EncumberHighThreshold">Where the HUD shows the player as heavily encumbered.</param>
internal readonly record struct EncumberThresholds(
    float MaxCarryCapacity,
    float MaxCarryCapacityWhenExhausted,
    float NoSprintCarryCapacity,
    float NoWalkCarryCapacity,
    float EncumberLowThreshold,
    float EncumberMedThreshold,
    float EncumberHighThreshold);
