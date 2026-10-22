using Il2CppTLD.IntBackedUnit;

using UniversalTweaks.Core;

namespace UniversalTweaks.Utilities;

/// <summary>
/// How much the player can carry, with the extra weight from the settings, and the calorie burn that goes with it.
/// </summary>
internal static class Encumbrance
{
    /// <summary>One kilogram as an <see cref="ItemWeight"/>, which stores whole internal units rather than kilograms.</summary>
    private static ItemWeight OneKilogram => ItemWeight.FromKilograms(1f);

    /// <summary>The extra weight, in kilograms, the player can carry with the current settings.</summary>
    internal static float AdditionalWeight =>
        EncumberCalculator.ResolveAdditionalWeight(Settings.Instance.AdditionalEncumbermentWeight,
            Settings.Instance.CheatingTweaks && Settings.Instance.InfiniteEncumberWeight);

    /// <summary>Sets the player's carry thresholds with <see cref="AdditionalWeight"/> added to each.</summary>
    /// <param name="encumber">The player's encumbrance component.</param>
    internal static void Apply(Encumber encumber)
    {
        var additionalWeight = AdditionalWeight;
        var thresholds = EncumberCalculator.CalculateThresholds(additionalWeight);

        encumber.m_MaxCarryCapacity = ItemWeight.FromKilograms(thresholds.MaxCarryCapacity);
        encumber.m_MaxCarryCapacityWhenExhausted = ItemWeight.FromKilograms(thresholds.MaxCarryCapacityWhenExhausted);
        encumber.m_NoSprintCarryCapacity = ItemWeight.FromKilograms(thresholds.NoSprintCarryCapacity);
        encumber.m_NoWalkCarryCapacity = ItemWeight.FromKilograms(thresholds.NoWalkCarryCapacity);
        encumber.m_EncumberLowThreshold = ItemWeight.FromKilograms(thresholds.EncumberLowThreshold);
        encumber.m_EncumberMedThreshold = ItemWeight.FromKilograms(thresholds.EncumberMedThreshold);
        encumber.m_EncumberHighThreshold = ItemWeight.FromKilograms(thresholds.EncumberHighThreshold);

        Mod.Logger.Log($"Applied encumbrance tweaks with {additionalWeight}KG additional weight.", FlaggedLoggingLevel.Debug);
    }

    /// <summary>
    /// Works out the player's calorie burn rate as the game does, but with the burn from carried weight scaled back to
    /// the game's own 30 kg capacity, so extra carry weight doesn't make every kilogram burn less.
    /// </summary>
    /// <param name="player">The player manager.</param>
    /// <param name="baseBurnRate">The burn rate before weight, cold, speed and difficulty, in calories an hour.</param>
    /// <returns>The burn rate, in calories an hour.</returns>
    internal static float CalorieBurnRate(PlayerManager player, float baseBurnRate)
    {
        var encumber = GameManager.GetEncumberComponent();
        var freezing = GameManager.GetFreezingComponent();
        var isMoving = player.PlayerIsSprinting() || player.PlayerIsWalking();
        var rate = baseBurnRate;

        if (isMoving || player.PlayerIsClimbing())
        {
            var maxCarryCapacityKG = encumber.m_MaxCarryCapacity.m_Units / (float)OneKilogram.m_Units;
            rate += encumber.GetHourlyCalorieBurnFromWeight() * (30f / maxCarryCapacityKG);
        }

        if (freezing.IsFreezing())
            rate *= freezing.m_CalorieBurnMultiplier;

        if (isMoving)
        {
            var playerVelocity = GameManager.GetVpFPSPlayer().Controller.Velocity.normalized.y;
            if (playerVelocity > 0.1f)
            {
                var speed = (playerVelocity - 0.1f) / 0.5f;
                speed = Mathf.Clamp(speed, 0f, 1f);
                var speedMod = Mathf.Lerp(1f, 1.5f, speed);
                rate *= speedMod;
            }
        }

        rate *= GameManager.GetExperienceModeManagerComponent().GetCalorieBurnScale();
        return rate * GameManager.GetFeatEfficientMachine().ReduceCaloriesScale();
    }
}
