using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class Noisemaker
{
    [HarmonyPatch(typeof(GearItem), nameof(GearItem.Deserialize))]
    private static class AdjustNoisemakerValues
    {
        private static void Postfix(GearItem __instance)
        {
            if (__instance.gameObject.name is not "GEAR_NoiseMaker")
            {
                return;
            }

            __instance.m_NoiseMakerItem.m_BurnLifetimeMinutes = Settings.Instance.NoisemakerBurnLength;
            __instance.m_NoiseMakerItem.m_ThrowForce = Settings.Instance.NoisemakerThrowForce;

            Mod.Logger.Log(
                $"Applied noisemaker tweaks: burn length {Settings.Instance.NoisemakerBurnLength}, throw force {Settings.Instance.NoisemakerThrowForce}.",
                FlaggedLoggingLevel.Debug);
        }
    }
}