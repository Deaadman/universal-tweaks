namespace UniversalTweaks.Core;

/// <summary>
/// The game's own values for the tweaks behind 'Cheat' Tweaks, put back whenever it's turned off.
/// </summary>
/// <remarks>Beam lights aren't here: they put back the values captured from each light instead.</remarks>
internal static class VanillaDefaults
{
    /// <summary>The flashlight's battery durations and recharge time.</summary>
    internal static readonly FlashlightTuning Flashlight = new(
        LowBeamDuration: 1f,
        HighBeamDuration: 0.08333334f,
        RechargeTime: 2f);

    /// <summary>The miner's flashlight's battery durations and recharge time.</summary>
    internal static readonly FlashlightTuning MinersFlashlight = Flashlight with
    {
        LowBeamDuration = 1.5f,
        RechargeTime = 1.75f,
    };

    /// <summary>How many hit points a snow shelter loses a day.</summary>
    internal const float SnowShelterDailyDecayHP = 100f;

    /// <summary>How many hit points a travois loses an hour in a blizzard.</summary>
    internal const float TravoisBlizzardDecayPerHour = 3f;

    /// <summary>How many hit points a travois loses an hour.</summary>
    internal const float TravoisDecayHPPerHour = 0.01f;

    /// <summary>How many hit points a travois loses for each unit of distance it's dragged.</summary>
    internal const float TravoisMovementDecayPerUnit = 0.05f;
}
