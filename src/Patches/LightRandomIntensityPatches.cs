namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="LightRandomIntensity"/>, which makes lights flicker, including the flashlight's beam.</summary>
internal static class LightRandomIntensityPatches
{
    /// <summary>
    /// Holds the flashlight's beam steady outside auroras when <see cref="Settings.AuroraFlickering"/> and
    /// <see cref="Settings.ExtendedFunctionality"/> are on.
    /// </summary>
    [HarmonyPatch(typeof(LightRandomIntensity), nameof(LightRandomIntensity.Update))]
    private static class FlashlightFlicker
    {
        private static bool Prefix(LightRandomIntensity __instance)
        {
            // Runs for every flickering light in the scene, so the name (a marshalled string) is read last.
            if (!Settings.Instance.AuroraFlickering || !Settings.Instance.ExtendedFunctionality ||
                GameManager.GetAuroraManager().AuroraIsActive())
                return true;

            if (__instance.gameObject.name is not ("LightIndoors" or "LightOutdoors" or "LightExtend"))
                return true;

            // Hold the beam at its base intensity instead of freezing on whatever the last flicker left.
            var light = __instance.m_Light != null ? __instance.m_Light.GetLightComponent() : null;
            if (light != null)
                light.intensity = __instance.m_LightIntensityStart;

            return false;
        }
    }
}
