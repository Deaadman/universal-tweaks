using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class SnowShelter
{
    [HarmonyPatch(typeof(SnowShelterManager), nameof(SnowShelterManager.InstantiateSnowShelter))]
    private static class SnowShelterDecayRate
    {
        private static void Postfix(ref Il2Cpp.SnowShelter __result)
        {
            __result.m_DailyDecayHP =
                Settings.Instance.CheatingTweaks ? Settings.Instance.SnowShelterDailyDecayRate : 100;

            Mod.Logger.Log($"Set snow shelter daily decay to {__result.m_DailyDecayHP}.", FlaggedLoggingLevel.Debug);
        }
    }
}