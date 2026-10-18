using UniversalTweaks.Properties;
using UniversalTweaks.Utilities;

namespace UniversalTweaks.Tweaks;

internal static class SnowShelterTweaks
{
    [HarmonyPatch(typeof(SnowShelterManager), nameof(SnowShelterManager.InstantiateSnowShelter))]
    private static class SnowShelterDecayRate
    {
        private static void Postfix(ref SnowShelter __result)
        {
            __result.m_DailyDecayHP = Settings.Instance.CheatingTweaks
                ? Settings.Instance.SnowShelterDailyDecayRate
                : VanillaDefaults.SnowShelterDailyDecayHP;

            Mod.Logger.Log($"Set snow shelter daily decay to {__result.m_DailyDecayHP}.", FlaggedLoggingLevel.Debug);
        }
    }
}