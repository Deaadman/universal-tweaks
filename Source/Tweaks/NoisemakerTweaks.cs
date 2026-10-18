using UniversalTweaks.Properties;

namespace UniversalTweaks.Tweaks;

internal static class NoisemakerTweaks
{
    // Applied each tick and on lifetime queries rather than on load, so setting changes reach existing noisemakers.
    [HarmonyPatch(typeof(NoiseMakerItem), nameof(NoiseMakerItem.Update))]
    private static class ApplyNoisemakerTweaksOnUpdate
    {
        private static void Prefix(NoiseMakerItem __instance) => ApplyNoisemakerTweaks(__instance);
    }

    [HarmonyPatch(typeof(NoiseMakerItem), nameof(NoiseMakerItem.GetModifiedBurnLifetimeMinutes))]
    private static class ApplyNoisemakerTweaksOnLifetimeQuery
    {
        private static void Prefix(NoiseMakerItem __instance) => ApplyNoisemakerTweaks(__instance);
    }

    private static void ApplyNoisemakerTweaks(NoiseMakerItem noisemaker)
    {
        noisemaker.m_BurnLifetimeMinutes = Settings.Instance.NoisemakerBurnLength;
        noisemaker.m_ThrowForce = Settings.Instance.NoisemakerThrowForce;
    }
}