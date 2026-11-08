using UniversalTweaks.Core;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="SnowShelter"/>.</summary>
internal static class SnowShelterPatches
{
    /// <summary>
    /// Sets how fast snow shelters decay from <see cref="Settings.SnowShelterDailyDecayRate"/> with 'Cheat' Tweaks on, or
    /// the game's own with it off. Applied every frame, so changes reach shelters that already exist.
    /// </summary>
    [HarmonyPatch(typeof(SnowShelter), nameof(SnowShelter.Update))]
    private static class SnowShelterDecayRate
    {
        private static void Prefix(SnowShelter __instance) =>
            __instance.m_DailyDecayHP = Settings.Instance.CheatingTweaks
                ? Settings.Instance.SnowShelterDailyDecayRate
                : VanillaDefaults.SnowShelterDailyDecayHP;
    }
}
