namespace UniversalTweaks.Utilities;

// The game's own values, restored whenever 'Cheat' Tweaks is disabled.
internal static class VanillaDefaults
{
    internal static readonly FlashlightTuning Flashlight = new(
        LowBeamDuration: 1f,
        HighBeamDuration: 0.08333334f,
        RechargeTime: 2f,
        LowBeamIntensity: 1f,
        LowBeamRange: 40f,
        HighBeamIntensity: 7.86f,
        HighBeamRange: 80f);

    internal static readonly FlashlightTuning MinersFlashlight = Flashlight with
    {
        LowBeamDuration = 1.5f,
        RechargeTime = 1.75f,
    };

    internal const float SnowShelterDailyDecayHP = 100f;

    internal const float TravoisBlizzardDecayPerHour = 3f;
    internal const float TravoisDecayHPPerHour = 0.01f;
    internal const float TravoisMovementDecayPerUnit = 0.05f;
}