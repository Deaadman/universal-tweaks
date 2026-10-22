namespace UniversalTweaks.Patches;

/// <summary>Patches on <see cref="Panel_SprayPaint"/>, the screen for picking a spray paint decal.</summary>
internal static class PanelSprayPaintPatches
{
    /// <summary>Sets how close decals can be placed to each other from <see cref="Settings.DecalOverlapLeniency"/>.</summary>
    [HarmonyPatch(typeof(Panel_SprayPaint), nameof(Panel_SprayPaint.Enable), [typeof(bool)])]
    private static class DecalRestrictions
    {
        private static void Postfix()
        {
            var dynamicDecalsManager = GameManager.GetDynamicDecalsManager();
            dynamicDecalsManager.m_DecalOverlapLeniencyPercent = Settings.Instance.DecalOverlapLeniency;

            Mod.Logger.Log($"Decal overlap leniency set to {Settings.Instance.DecalOverlapLeniency}.", FlaggedLoggingLevel.Debug);
        }
    }
}
