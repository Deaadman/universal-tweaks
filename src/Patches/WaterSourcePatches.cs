using UniversalTweaks.Utilities;

namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="WaterSource"/>, places the player can take water from, like toilets.</summary>
internal static class WaterSourcePatches
{
    /// <summary>The "Potable" choice of <see cref="Settings.ToiletWaterQuality"/>.</summary>
    private const int PotableChoice = 0;

    /// <summary>The "Vanilla" choice of <see cref="Settings.ToiletWaterQuality"/>.</summary>
    /// <remarks>The settings file saves choices by position, so Vanilla was added last rather than first.</remarks>
    private const int VanillaChoice = 2;

    /// <summary>The game's own quality of each source, captured before the first change so choosing Vanilla puts it back.</summary>
    private static readonly ObjectStateCache<WaterSource, LiquidQuality> OriginalQualities = new();

    /// <summary>Makes toilet water potable or not from <see cref="Settings.ToiletWaterQuality"/>.</summary>
    [HarmonyPatch(typeof(WaterSource), nameof(WaterSource.Update))]
    private static class ToiletWaterQuality
    {
        private static void Postfix(WaterSource __instance)
        {
            var choice = Settings.Instance.ToiletWaterQuality;

            if (choice == VanillaChoice)
            {
                if (OriginalQualities.Remove(__instance, out var original))
                    __instance.m_CurrentLiquidQuality = original;

                return;
            }

            OriginalQualities.TryAdd(__instance, __instance.m_CurrentLiquidQuality);
            __instance.m_CurrentLiquidQuality = choice == PotableChoice ? LiquidQuality.Potable : LiquidQuality.NonPotable;
        }
    }
}
