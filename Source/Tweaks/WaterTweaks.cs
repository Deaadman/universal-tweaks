using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class WaterTweaks
{
    [HarmonyPatch(typeof(WaterSource), nameof(WaterSource.Update))]
    private static class NonPotableToiletWater
    {
        private static void Postfix(WaterSource __instance)
        {
            __instance.m_CurrentLiquidQuality = Settings.Instance.ToiletWaterQuality == 1
                ? LiquidQuality.NonPotable
                : LiquidQuality.Potable;
        }
    }
}