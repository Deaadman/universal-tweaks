namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="HUDManager"/>, which shows and hides parts of the HUD.</summary>
internal static class HUDManagerPatches
{
    /// <summary>Keeps the crosshair showing when <see cref="Settings.PermanentCrosshair"/> is on.</summary>
    [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.UpdateCrosshair))]
    private static class PermanentCrosshair
    {
        private static void Prefix(HUDManager __instance)
        {
            if (Settings.Instance.PermanentCrosshair)
                __instance.m_CrosshairAlpha = 1f;
        }
    }
}
