namespace UniversalTweaks.Utilities;

internal static class EncumberCalculator
{
    internal static float ResolveAdditionalWeight(int additionalEncumbermentWeight, bool infiniteEncumberWeight)
    {
        if (infiniteEncumberWeight)
        {
            return 9970f;
        }

        return additionalEncumbermentWeight > 0 ? additionalEncumbermentWeight : 0f;
    }

    internal static EncumberThresholds CalculateThresholds(float additionalWeight)
    {
        return new EncumberThresholds(
            MaxCarryCapacity: 30f + additionalWeight,
            MaxCarryCapacityWhenExhausted: 15f + additionalWeight,
            NoSprintCarryCapacity: 40f + additionalWeight,
            NoWalkCarryCapacity: 60f + additionalWeight,
            EncumberLowThreshold: 31f + additionalWeight,
            EncumberMedThreshold: 40f + additionalWeight,
            EncumberHighThreshold: 60f + additionalWeight);
    }
}

internal readonly record struct EncumberThresholds(
    float MaxCarryCapacity,
    float MaxCarryCapacityWhenExhausted,
    float NoSprintCarryCapacity,
    float NoWalkCarryCapacity,
    float EncumberLowThreshold,
    float EncumberMedThreshold,
    float EncumberHighThreshold);