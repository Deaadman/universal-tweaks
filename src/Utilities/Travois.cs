using Il2CppTLD.BigCarry;

using UniversalTweaks.Core;

namespace UniversalTweaks.Utilities;

/// <summary>Applies the travois settings: turning speed, slope angle and decay.</summary>
internal static class Travois
{
    /// <summary>Applies the settings to every travois in the scene, including one already placed.</summary>
    internal static void Refresh()
    {
        foreach (var travois in UnityEngine.Object.FindObjectsOfType<TravoisBigCarryItem>())
            Apply(travois);
    }

    /// <summary>
    /// Sets a travois's turning speed and slope angle from the settings, and its decay from the settings with 'Cheat'
    /// Tweaks on or the game's own with it off.
    /// </summary>
    /// <param name="travois">The travois.</param>
    internal static void Apply(TravoisBigCarryItem travois)
    {
        var settings = Settings.Instance;

        var movement = travois.m_TravoisMovement;
        if (movement != null)
        {
            movement.m_TurnSpeed = settings.TurnSpeedTravois;
            movement.m_MaxSlopeClimbAngle = settings.MaximumSlopeAngleTravois;
            movement.m_MaxSlopeDownhillAngle = settings.MaximumSlopeAngleTravois;
        }

        travois.m_BlizzardDecayPerHour = settings.CheatingTweaks
            ? settings.DecayBlizzardTravois
            : VanillaDefaults.TravoisBlizzardDecayPerHour;
        travois.m_DecayHPPerHour = settings.CheatingTweaks
            ? settings.DecayHPPerHourTravois / 1000f
            : VanillaDefaults.TravoisDecayHPPerHour;
        travois.m_MovementDecayPerUnit = settings.CheatingTweaks
            ? settings.DecayMovementPerUnitTravois / 100f
            : VanillaDefaults.TravoisMovementDecayPerUnit;
    }
}
