using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class WaterTweaks
{
    // Choice indices are saved in the settings file, so Vanilla was appended rather than placed first.
    private const int PotableChoice = 0;
    private const int VanillaChoice = 2;

    // The game's own quality per source, captured before the first override so choosing Vanilla restores it.
    private static readonly Dictionary<int, LiquidQuality> OriginalQualities = new();

    [HarmonyPatch(typeof(WaterSource), nameof(WaterSource.Update))]
    private static class ToiletWaterQuality
    {
        private static void Postfix(WaterSource __instance)
        {
            var id = __instance.GetInstanceID();
            var choice = Settings.Instance.ToiletWaterQuality;

            if (choice == VanillaChoice)
            {
                if (OriginalQualities.Remove(id, out var original))
                {
                    __instance.m_CurrentLiquidQuality = original;
                }

                return;
            }

            OriginalQualities.TryAdd(id, __instance.m_CurrentLiquidQuality);
            __instance.m_CurrentLiquidQuality = choice == PotableChoice ? LiquidQuality.Potable : LiquidQuality.NonPotable;
        }
    }
}