using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class SnowShelterTweaks
{
    // Applied each tick rather than on creation so setting changes reach shelters that already exist.
    [HarmonyPatch(typeof(SnowShelter), nameof(SnowShelter.Update))]
    private static class SnowShelterDecayRate
    {
        private static void Prefix(SnowShelter __instance)
        {
            __instance.m_DailyDecayHP = Settings.Instance.CheatingTweaks
                ? Settings.Instance.SnowShelterDailyDecayRate
                : VanillaDefaults.SnowShelterDailyDecayHP;
        }
    }
}