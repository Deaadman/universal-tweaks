using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class FeatProgress
{
    [HarmonyPatch(typeof(Feat), nameof(Feat.ShouldBlockIncrement))]
    private static class FeatProgressInCustomMode
    {
        private static void Postfix(ref bool __result)
        {
            if (Settings.Instance.FeatProgressInCustomMode)
            {
                __result = false;
                Mod.Logger.Log("Allowing feat progress in custom mode.", FlaggedLoggingLevel.Trace);
            }
        }
    }
}