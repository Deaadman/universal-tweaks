namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="NoiseMakerItem"/>, the noisemaker.</summary>
/// <remarks>
/// The settings are applied every frame and whenever the fuse is read, rather than once on load, so changes reach
/// noisemakers that already exist.
/// </remarks>
internal static class NoiseMakerItemPatches
{
    /// <summary>Applies the noisemaker settings every frame.</summary>
    [HarmonyPatch(typeof(NoiseMakerItem), nameof(NoiseMakerItem.Update))]
    private static class ApplyNoisemakerTweaksOnUpdate
    {
        private static void Prefix(NoiseMakerItem __instance) => ApplyNoisemakerTweaks(__instance);
    }

    /// <summary>Applies the noisemaker settings before its fuse length is read.</summary>
    [HarmonyPatch(typeof(NoiseMakerItem), nameof(NoiseMakerItem.GetModifiedBurnLifetimeMinutes))]
    private static class ApplyNoisemakerTweaksOnLifetimeQuery
    {
        private static void Prefix(NoiseMakerItem __instance) => ApplyNoisemakerTweaks(__instance);
    }

    /// <summary>Sets a noisemaker's fuse length and throw force from the settings.</summary>
    /// <param name="noisemaker">The noisemaker.</param>
    private static void ApplyNoisemakerTweaks(NoiseMakerItem noisemaker)
    {
        noisemaker.m_BurnLifetimeMinutes = Settings.Instance.NoisemakerBurnLength;
        noisemaker.m_ThrowForce = Settings.Instance.NoisemakerThrowForce;
    }
}
